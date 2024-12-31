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
using EHRM.ServiceLayer.Helpers;
using Newtonsoft.Json;
using EHRM.ViewModel.MainMenu;
using EHRM.ViewModel.SubMenu;
using EHRM.ServiceLayer.Utility;


namespace EHRM.Web.Controllers
{
    public class AccountController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly EhrmContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IServiceProvider _serviceProvider;

        public AccountController(EhrmContext context, IWebHostEnvironment webHostEnvironment, IServiceProvider serviceProvider)
        {
            //_logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _serviceProvider = serviceProvider;
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
                TempData["ToastType"] = "danger";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Please fill out all fields correctly!";
                return RedirectToAction("Login");
            }

            var employeeCred = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == model.Email && e.Active == true);
            if (employeeCred == null)
            {
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Invalid login attempt. Email not found.!";
                return RedirectToAction("Login");
            }

            if (employeeCred.IsLockedOut && !IsLockoutExpired(employeeCred))
            {
                var minutesLeft = GetLockoutTimeRemaining(employeeCred);
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = $"Your account is locked. Try again in {minutesLeft} minutes or contact the administrator.";
                return RedirectToAction("Login");
            }

            if (employeeCred.IsLockedOut)
            {
                ResetLockout(employeeCred);
            }

            if (employeeCred.TempPassword != model.Password)
            {
                await HandleFailedLoginAttempt(employeeCred);
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Invalid login attempt. Your account may be locked after multiple attempts.";
                return RedirectToAction("Login");
            }

            await HandleSuccessfulLogin(employeeCred);

            // Generate JWT Token
            string jwtToken = JwtSessionHelper.GenerateJwtToken(
                employeeCred.EmpId.ToString(),
                employeeCred.Email,
                employeeCred.FirstName+" "+employeeCred.LastName,
                employeeCred.RoleId
            );

            // Store JWT token in session
            HttpContext.Session.SetString("JwtToken", jwtToken);
            HttpContext.Response.Headers.Add("Authorization", $"Bearer {jwtToken}");

            string otp = GenerateOtp();
            await SendOtp(employeeCred.Email, otp);

            HttpContext.Session.SetString("Otp", otp);
            HttpContext.Session.SetString("OtpExpiry", DateTime.Now.AddMinutes(3).ToString()); // Store OTP expiry
            TempData["ToastType"] = "Success";  // Success, danger, warning, info
            TempData["ToastMessage"] = "Login successful. Please enter the OTP sent to your registered email.";
            return RedirectToAction("Otp", "Account");
        }

        //public async Task<IActionResult> SaveLogin(LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["ErrorMessage"] = "Please fill out all fields correctly.";
        //        return RedirectToAction("Login");
        //    }

        //    var employeeCred = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == model.Email && e.Active == true);
        //    if (employeeCred == null)
        //    {
        //        TempData["ErrorMessage"] = "Invalid login attempt. Email not found.";
        //        return RedirectToAction("Login");
        //    }

        //    if (employeeCred.IsLockedOut && !IsLockoutExpired(employeeCred))
        //    {
        //        var minutesLeft = GetLockoutTimeRemaining(employeeCred);
        //        TempData["ErrorMessage"] = $"Your account is locked. Try again in {minutesLeft} minutes or contact the administrator.";
        //        return RedirectToAction("Login");
        //    }

        //    if (employeeCred.IsLockedOut)
        //    {
        //        ResetLockout(employeeCred);
        //    }

        //    if (employeeCred.TempPassword != model.Password)
        //    {
        //        await HandleFailedLoginAttempt(employeeCred);
        //        TempData["ErrorMessage"] = "Invalid login attempt. Your account may be locked after multiple attempts.";
        //        return RedirectToAction("Login");
        //    }

        //    await HandleSuccessfulLogin(employeeCred);

        //    string otp = GenerateOtp();
        //    await SendOtp(employeeCred.Email, otp);

        //    HttpContext.Session.SetString("Otp", otp);
        //    HttpContext.Session.SetString("OtpExpiry", DateTime.Now.AddMinutes(3).ToString()); // Store OTP expiry
        //    TempData["SuccessMessage"] = "Login successful. Please enter the OTP sent to your registered email.";

        //    return RedirectToAction("Otp", "Account");
        //}


        private bool IsLockoutExpired(EmployeesCred employeeCred)
        {
            return employeeCred.LockoutEndTime.HasValue && employeeCred.LockoutEndTime.Value <= DateTime.Now;
        }

        private int GetLockoutTimeRemaining(EmployeesCred employeeCred)
        {
            if (employeeCred.LockoutEndTime.HasValue)
            {
                var timeLeft = employeeCred.LockoutEndTime.Value - DateTime.Now;
                return (int)Math.Ceiling(timeLeft.TotalMinutes);
            }
            return 0;
        }

        private void ResetLockout(EmployeesCred employeeCred)
        {
            employeeCred.IsLockedOut = false;
            employeeCred.FailedLoginAttempts = 0;
            employeeCred.LockoutEndTime = null;
        }

        private async Task HandleFailedLoginAttempt(EmployeesCred employeeCred)
        {
            employeeCred.FailedLoginAttempts++;
            if (employeeCred.FailedLoginAttempts >= 3)
            {
                employeeCred.IsLockedOut = true;
                employeeCred.LockoutEndTime = DateTime.Now.AddMinutes(15);
            }
            await _context.SaveChangesAsync();
        }

        private async Task HandleSuccessfulLogin(EmployeesCred employeeCred)
        {
            employeeCred.FailedLoginAttempts = 0;
            employeeCred.IsLockedOut = false;
            employeeCred.LockoutEndTime = null;
            await _context.SaveChangesAsync();
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
        private async Task<string> GetTemplateFromFile(string templateName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", $"{templateName}.html");
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException($"Template '{templateName}' not found at '{filePath}'");
            }
            return await System.IO.File.ReadAllTextAsync(filePath);
        }
        private async Task SendOtp(string email, string otp)
        {
            EmailService otpemail = new EmailService();
            string template = await GetTemplateFromFile("OTPEmailTemplate");
            string body = template.Replace("{{otp}}", otp);

            string subject = "Your OTP Code";
            //string body = $"<p>Your OTP code is: <strong>{otp}</strong></p><p>Please use this code to complete your login.</p>";
            //string logoPath = Path.Combine(_webHostEnvironment.WebRootPath, "pic", "logo.png");

            // Call the SendEmail method to send the OTP
            otpemail.SendEmail(email, subject, body);
        }
        [HttpPost]
        public async Task<IActionResult> Otp(OtpViewModel model)
        {
            var otpExpiry = HttpContext.Session.GetString("OtpExpiry");
            if (otpExpiry != null && DateTime.TryParse(otpExpiry, out DateTime expiry) && expiry < DateTime.Now)
            {
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Your OTP has expired. Please request a new one.";
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Please enter a valid OTP.";
                return RedirectToAction("Otp");
            }

            var storedOtp = HttpContext.Session.GetString("Otp");

            if (string.IsNullOrEmpty(storedOtp))
            {
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = "OTP is not available. Please request a new one.";
                return RedirectToAction("Otp");
            }

            if (model.Otp != storedOtp)
            {
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Invalid OTP. Please try again.";
                return RedirectToAction("Otp");
            }
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                TempData["ToastType"] = "danger";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            var (userId, userName, email, roleId) = JwtSessionHelper.ExtractSessionData(jwtToken);

            var employee = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == email && e.Active == true);
            if (employee == null || !employee.Active)
            {
                TempData["ToastType"] = "danger";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Employee not found or inactive. Please contact support.";
                return RedirectToAction("Login");
            }

            HttpContext.Session.Remove("Otp");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtToken);

            var subMenus = await _context.SubMenus
                  .Where(x => x.RoleId == roleId && x.EmpId == employee.EmpId)
                  .ToListAsync();

            var mainMenuIds = subMenus.Select(x => x.MainMenuId).Distinct().ToList();
            var mainMenus = await _context.MainMenus.Where(m => mainMenuIds.Contains(m.Id)).ToListAsync();

            var groupedSubMenusWithMainMenu = subMenus
                .GroupBy(x => x.MainMenuId)
                .Select(g => new MainMenuViewModel
                {
                    Id = g.Key ?? 0, // Id can be nullable (int?)
                    Name = mainMenus.FirstOrDefault(m => m.Id == g.Key)?.Name,
                    Icon = mainMenus.FirstOrDefault(m => m.Id == g.Key)?.Icon,
                    SubMenus = g.Select(submenu => new SubMenuViewModel
                    {
                        Id = submenu.Id,
                        Name = submenu.Name,
                        Controller = submenu.Controller,
                        Action = submenu.Action,
                        MainMenuId = submenu.MainMenuId,
                        MainMenuName = mainMenus.FirstOrDefault(m => m.Id == submenu.MainMenuId)?.Name,
                        RoleId = submenu.RoleId,
                        //RoleName = submenu.RoleName,
                        IsActive = submenu.IsActive
                    }).ToList() // Map SubMenu to SubMenuViewModel
                })
                .ToList();

            // Serialize and store in session as before
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var jsonString = JsonConvert.SerializeObject(groupedSubMenusWithMainMenu, jsonSettings);

            var jsonFoUserDetails = JsonConvert.SerializeObject(userDetails);
            HttpContext.Session.SetString("GroupByUserDetails", jsonFoUserDetails);
            HttpContext.Session.SetString("GroupedSubMenus", jsonString);



            return RedirectToAction("Dashboard", "Dashboard");
        }


        //[HttpPost]
        //public async Task<IActionResult> Otp(OtpViewModel model)
        //{
        //    var otpExpiry = HttpContext.Session.GetString("OtpExpiry");
        //    if (otpExpiry != null && DateTime.Parse(otpExpiry) < DateTime.Now)
        //    {
        //        TempData["ErrorMessage"] = "Your OTP has expired. Please request a new one.";
        //        return RedirectToAction("Login");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["ErrorMessage"] = "Please enter a valid OTP.";
        //        return RedirectToAction("Otp"); // Return to OTP page with validation errors
        //    }

        //    // Retrieve the stored OTP from the session
        //    var storedOtp = HttpContext.Session.GetString("Otp");

        //    // Verify the OTP
        //    if (string.IsNullOrEmpty(storedOtp) || model.Otp != storedOtp)
        //    {
        //        TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
        //        return RedirectToAction("Otp");
        //    }

        //    // Get the employee using the email passed in the model
        //    var employee = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == model.Email && e.Active == true);

        //    if (employee != null)
        //    {
        //        // Create claims for the authenticated employee
        //        var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Email, employee.Email ?? string.Empty),
        //                new Claim(ClaimTypes.NameIdentifier, employee.EmpId.ToString()),
        //                new Claim("EmpId", employee.EmpId.ToString()),
        //                new Claim("RoleId", employee.RoleId?.ToString() ?? string.Empty),
        //                new Claim("FirstName", employee.FirstName?.Trim() ?? string.Empty),
        //                new Claim("LastName", employee.LastName?.Trim() ?? string.Empty),
        //                new Claim("Email", employee.Email?.Trim() ?? string.Empty)
        //            };

        //        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


        //        // Clear OTP from the session
        //        HttpContext.Session.Remove("Otp");
        //        // Retrieve RoleId from AppUser
        //        // Get the RoleId for the current user (ensure this method works asynchronously)


        //        // Assuming you have access to the DbContext and the current employee's EmpId
        //        /* Retrieve the current EmpId, possibly from user claims or session */

        //        // Fetch the submenus with RoleId and EmpId filter
        //        var SubMenus = await _context.SubMenus
        //            .Where(x => x.RoleId == RoleId && x.EmpId == currentEmpId) // Filter by RoleId and EmpId
        //            .ToListAsync();

        //        // Group SubMenus by MainMenuId
        //        var groupByMainMenu = SubMenus
        //            .GroupBy(x => x.MainMenuId) // Group by MainMenuId
        //            .ToList();

        //        // Optionally, retrieve MainMenu names if needed
        //        var groupedSubMenusWithMainMenu = groupByMainMenu
        //            .Select(g => new
        //            {
        //                MainMenuId = g.Key,
        //                MainMenuName = _context.MainMenus.FirstOrDefault(m => m.Id == g.Key)?.Name,
        //                SubMenus = g.ToList() // SubMenus under each MainMenu
        //            })
        //            .ToList();
        //        // Store the grouped data as a JSON string in the session
        //        HttpContext.Session.SetString("GroupedSubMenus", JsonConvert.SerializeObject(groupedSubMenusWithMainMenu));
        //        // Redirect to the dashboard or welcome page
        //        return RedirectToAction("AddHoliday", "Master");
        //    }

        //    // If employee not found
        //    TempData["ErrorMessage"] = "An error occurred. Please try again.";
        //    return RedirectToAction("Login");
        //}


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            // Clear the JWT token stored in the session (if any)
            HttpContext.Session.Remove("JwtToken");
            // Clear other session data if necessary
            HttpContext.Session.Clear();
            // Redirect the user to the login page or another appropriate page
            return RedirectToAction("Login", "Account");
        }
    }
}


