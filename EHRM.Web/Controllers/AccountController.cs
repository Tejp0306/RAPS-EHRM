using EHRM.ViewModel.Master;
using EHRM.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;
using System.Security.Claims;
using EHRM.ServiceLayer.Master;
using Microsoft.EntityFrameworkCore;
using EHRM.DAL.Database;
using EHRM.Web.Utility;


namespace EHRM.Web.Controllers
{
    public class AccountController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly EhrmContext _employee;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(EhrmContext employee, IWebHostEnvironment webHostEnvironment)
        {
            //_logger = logger;
            _employee = employee;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> SaveLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill out all fields correctly.";
                return RedirectToAction("Login"); // Return the same view with validation errors
            }

            var employee = await _employee.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == model.Email && e.Active == true);
            if (employee == null)
            {
                TempData["ErrorMessage"] = "Invalid login attempt. Email not found.";
                return RedirectToAction("Login");
            }

            if (employee.IsLockedOut == true)
            {
                if (employee.LockoutEndTime.HasValue && employee.LockoutEndTime.Value > DateTime.Now)
                {
                    var timeLeft = employee.LockoutEndTime.Value - DateTime.Now;
                    var minutesLeft = (int)Math.Ceiling(timeLeft.TotalMinutes);
                    TempData["ErrorMessage"] = $"Your account has been temporarily locked due to multiple unsuccessful login attempts. Please try again in {minutesLeft} minutes. If you need immediate assistance, contact your administrator.";
                    return RedirectToAction("Login");
                }
                else
                {
                    employee.IsLockedOut = false;
                    employee.FailedLoginAttempts = 0;
                    employee.LockoutEndTime = null;
                }
            }

            if (employee.TempPassword != model.Password)
            {
                employee.FailedLoginAttempts++;

                if (employee.FailedLoginAttempts >= 3)
                {
                    employee.IsLockedOut = true;
                    employee.LockoutEndTime = DateTime.Now.AddMinutes(15);
                }

                await _employee.SaveChangesAsync();
                TempData["ErrorMessage"] = "Invalid login attempt. Your account may be locked after multiple attempts.";
                return RedirectToAction("Login");
            }

            // Successful temporary password authentication
            employee.FailedLoginAttempts = 0;
            employee.IsLockedOut = false;
            employee.LockoutEndTime = null;
            await _employee.SaveChangesAsync();

            //Generate and send OTP
            string otp = GenerateOtp();
            await SendOtp(employee.Email, otp);

            // Store OTP in TempData or session
            HttpContext.Session.SetString("Otp", otp);  // Store OTP in session

            // Set success message
            TempData["SuccessMessage"] = "Login successful. Please enter the OTP sent to your registered email.";

            // Redirect to OTP verification page
            return RedirectToAction("Otp", "Account"); // Ensure you have an OTP action in your Account controller
            }
        public IActionResult Otp()
        {
            return View();
        }
        //Example method for generating OTP
        private string GenerateOtp()
        {
            // Implement OTP generation logic (e.g., random number, time-based, etc.)
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Example: 6-digit OTP
        }

        //Example method for sending OTP
            private async Task SendOtp(string email, string otp)
        {
            EmailService otpemail = new EmailService();
            string subject = "Your OTP Code";
            string body = $"<p>Your OTP code is: <strong>{otp}</strong></p><p>Please use this code to complete your login.</p>";
            string logoPath = Path.Combine(_webHostEnvironment.WebRootPath, "pic", "logo.png");

            // Call the SendEmail method to send the OTP
            otpemail.SendEmail(email, subject, body, logoPath);
        }

        [HttpPost]
        public async Task<IActionResult> Otp(OtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please enter a valid OTP.";
                return RedirectToAction("Otp"); // Return to OTP page with validation errors
            }

            // Retrieve the stored OTP from the session
            var storedOtp = HttpContext.Session.GetString("Otp");

            // Verify the OTP
            if (storedOtp == null || model.Otp != storedOtp)
            {
                TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
                return RedirectToAction("Otp");
            }

            // Get the employee (you might want to pass the employee's email to this method)
            var employee = await _employee.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == model.Email && e.Active == true);

            if (employee != null)
            {
                // Create claims for the authenticated employee
                var claims = new List<Claim>
           {
               new Claim(ClaimTypes.Email, employee.Email ?? string.Empty),
               new Claim(ClaimTypes.NameIdentifier, employee.EmpId.ToString()),
               //new Claim("FullName", $"{employee.FirstName ?? string.Empty} {employee.LastName ?? string.Empty}"),
               //new Claim("Position", employee.Position ?? string.Empty),
               new Claim("EmpId", employee.EmpId.ToString()),
               //new Claim("IsProfilecomplte", employee.IsProfilecomplte.ToString()),
               new Claim("Roleid",$"{employee.RoleId.ToString() ?? string.Empty}")
           };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Sign in the user
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                //var setusers = UserClaimsHelper.GetUserClaims(User);
        //var roleId = setusers["Roleid"];
                // Redirect to the dashboard or welcome page
                return RedirectToAction("Welcome", "Dashboard");
    }

    // If employee not found
    TempData["ErrorMessage"] = "An error occurred. Please try again.";
            return RedirectToAction("Login");
}

[HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Optionally clear other cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                // Remove each cookie
                Response.Cookies.Delete(cookie);
            }

            // Redirect to the login page or another page
            return RedirectToAction("Login", "Home");
        }
    }
}


