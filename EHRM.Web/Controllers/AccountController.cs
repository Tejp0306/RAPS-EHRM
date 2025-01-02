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
using NuGet.Common;
using Newtonsoft.Json.Linq;


namespace EHRM.Web.Controllers
{
    public class AccountController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly EhrmContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration; 

        public AccountController(EhrmContext context, IWebHostEnvironment webHostEnvironment, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            //_logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
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
            try
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
                    TempData["ToastMessage"] = "Invalid login attempt. Email not found.";
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

                // Skip JWT Token Generation here, handle in OTP section
                // Instead, store the employee data for OTP verification
                HttpContext.Session.SetString("EmployeeEmail", employeeCred.Email);

                // Check the MagicOTP configuration
                var magicOtpConfig = _configuration.GetSection("MagicOTP");
                bool key = magicOtpConfig.GetValue<bool>("key");

                if (!key)
                {
                    // Generate OTP and send email
                    string otp = GenerateOtp();
                    await SendOtp(employeeCred.Email, otp);

                    HttpContext.Session.SetString("Otp", otp);
                    HttpContext.Session.SetString("OtpExpiry", DateTime.Now.AddMinutes(3).ToString()); // Store OTP expiry
                    TempData["ToastType"] = "Success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Login successful. Please enter the OTP sent to your registered email.";
                }
                else
                {
                    // Magic OTP is enabled, skip OTP generation
                    TempData["ToastType"] = "Success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Login successful. Since Magic OTP is enabled, no OTP will be sent.";
                }

                return RedirectToAction("Otp", "Account");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public async Task<IActionResult> SaveLogin(LoginViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            TempData["ToastType"] = "danger";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Please fill out all fields correctly!";
        //            return RedirectToAction("Login");
        //        }

        //        var employeeCred = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == model.Email && e.Active == true);
        //        if (employeeCred == null)
        //        {
        //            TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Invalid login attempt. Email not found.";
        //            return RedirectToAction("Login");
        //        }

        //        if (employeeCred.IsLockedOut && !IsLockoutExpired(employeeCred))
        //        {
        //            var minutesLeft = GetLockoutTimeRemaining(employeeCred);
        //            TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = $"Your account is locked. Try again in {minutesLeft} minutes or contact the administrator.";
        //            return RedirectToAction("Login");
        //        }

        //        if (employeeCred.IsLockedOut)
        //        {
        //            ResetLockout(employeeCred);
        //        }

        //        if (employeeCred.TempPassword != model.Password)
        //        {
        //            await HandleFailedLoginAttempt(employeeCred);
        //            TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Invalid login attempt. Your account may be locked after multiple attempts.";
        //            return RedirectToAction("Login");
        //        }

        //        await HandleSuccessfulLogin(employeeCred);

        //        // Generate JWT Token
        //        string jwtToken = JwtSessionHelper.GenerateJwtToken(
        //            employeeCred.EmpId.ToString(),
        //            employeeCred.Email,
        //            employeeCred.FirstName + " " + employeeCred.LastName,
        //            employeeCred.RoleId
        //        );

        //        // Store token in a cookie
        //        HttpContext.Response.Cookies.Append("JwtToken", jwtToken, new CookieOptions
        //        {
        //            HttpOnly = true, // Prevent access from client-side scripts
        //            Secure = true,  // Use HTTPS in production
        //            SameSite = SameSiteMode.Strict
        //        });

        //        // Store JWT token in session
        //        HttpContext.Session.SetString("JwtToken", jwtToken);
        //        // Check the MagicOTP configuration
        //        var magicOtpConfig = _configuration.GetSection("MagicOTP");
        //        bool key = magicOtpConfig.GetValue<bool>("key");

        //        if (!key)
        //        {
        //            // Generate OTP and send email
        //            string otp = GenerateOtp();
        //            await SendOtp(employeeCred.Email, otp);

        //            HttpContext.Session.SetString("Otp", otp);
        //            HttpContext.Session.SetString("OtpExpiry", DateTime.Now.AddMinutes(3).ToString()); // Store OTP expiry
        //            TempData["ToastType"] = "Success";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Login successful. Please enter the OTP sent to your registered email.";
        //        }
        //        else
        //        {
        //            // Magic OTP is enabled, skip OTP generation
        //            TempData["ToastType"] = "Success";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Login successful. Since Magic OTP is enabled, no OTP will be sent.";
        //        }

        //        return RedirectToAction("Otp", "Account");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
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
            try
            {
                // Get MagicOTP key from configuration
                var magicOtpConfig = _configuration.GetSection("MagicOTP");
                bool key = magicOtpConfig.GetValue<bool>("key");
                string magicOTP = magicOtpConfig.GetValue<string>("Otp");

                // If MagicOTP.key is true, skip OTP validation and proceed with login
                if (key)
                {
                    if (model.Otp != magicOTP)
                    {
                        TempData["ToastType"] = "warning";  // Success, danger, warning, info
                        TempData["ToastMessage"] = "Invalid Magic OTP. Please try again.";
                        return RedirectToAction("Otp");
                    }

                    var employeeEmail = HttpContext.Session.GetString("EmployeeEmail");
                    if (string.IsNullOrEmpty(employeeEmail))
                    {
                        TempData["ToastType"] = "danger";  // Success, danger, warning, info
                        TempData["ToastMessage"] = "Session expired. Please log in again.";
                        return RedirectToAction("Login");
                    }

                    var employee = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == employeeEmail && e.Active == true);
                    if (employee == null || !employee.Active)
                    {
                        TempData["ToastType"] = "danger";  // Success, danger, warning, info
                        TempData["ToastMessage"] = "Employee not found or inactive. Please contact support.";
                        return RedirectToAction("Login");
                    }

                    // Generate JWT Token after OTP verification
                    string jwtToken = JwtSessionHelper.GenerateJwtToken(
                        employee.EmpId.ToString(),
                        employee.Email,
                        employee.FirstName + " " + employee.LastName,
                        employee.RoleId
                    );

                    // Store token in both cookie and session
                    StoreToken(jwtToken);

                    // Proceed to main menu handling
                    var subMenus = await _context.SubMenus
                         .Where(x => x.RoleId == employee.RoleId && x.EmpId == employee.EmpId)
                         .ToListAsync();

                    var mainMenuIds = subMenus.Select(x => x.MainMenuId).Distinct().ToList();
                    var mainMenus = await _context.MainMenus.Where(m => mainMenuIds.Contains(m.Id)).ToListAsync();

                    var groupedSubMenusWithMainMenu = subMenus
                       .GroupBy(x => x.MainMenuId)
                       .Select(g => new MainMenuViewModel
                       {
                           Id = g.Key ?? 0,
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
                               IsActive = submenu.IsActive
                           }).ToList()
                       })
                       .ToList();

                    var jsonSettings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    var jsonString = JsonConvert.SerializeObject(groupedSubMenusWithMainMenu, jsonSettings);

                    var jsonFoUserDetails = JsonConvert.SerializeObject(employee);
                    HttpContext.Session.SetString("GroupByUserDetails", jsonFoUserDetails);
                    HttpContext.Session.SetString("GroupedSubMenus", jsonString);

                    return RedirectToAction("Dashboard", "Dashboard");
                }

                // OTP handling when MagicOTP.key is not enabled
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
                var employeeEmails = HttpContext.Session.GetString("EmployeeEmail");
                var employees = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == employeeEmails && e.Active == true);

                string jwtTokens = JwtSessionHelper.GenerateJwtToken(
                     employees.EmpId.ToString(),
                     employees.Email,
                     employees.FirstName + " " + employees.LastName,
                     employees.RoleId
                 );
                // Store token in both cookie and session
                StoreToken(jwtTokens);
                //var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
                
                //if (string.IsNullOrEmpty(jwtTokenFromSession))
                //{
                //    TempData["ToastType"] = "danger";  // Success, danger, warning, info
                //    TempData["ToastMessage"] = "Session expired. Please log in again.";
                //    return RedirectToAction("Login");
                //}

                //var (userId, userName, email, roleIdFromSession) = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

               // var employeeFromSession = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == email && e.Active == true);
                //if (employeeFromSession == null || !employeeFromSession.Active)
                //{
                //    TempData["ToastType"] = "danger";  // Success, danger, warning, info
                //    TempData["ToastMessage"] = "Employee not found or inactive. Please contact support.";
                //    return RedirectToAction("Login");
                //}
              
                HttpContext.Session.Remove("Otp");

                var subMenusFromSession = await _context.SubMenus
                     .Where(x => x.RoleId == employees.RoleId && x.EmpId == employees.EmpId)
                     .ToListAsync();

                var mainMenuIdsFromSession = subMenusFromSession.Select(x => x.MainMenuId).Distinct().ToList();
                var mainMenusFromSession = await _context.MainMenus.Where(m => mainMenuIdsFromSession.Contains(m.Id)).ToListAsync();

                var groupedSubMenusWithMainMenuFromSession = subMenusFromSession
                   .GroupBy(x => x.MainMenuId)
                   .Select(g => new MainMenuViewModel
                   {
                       Id = g.Key ?? 0,
                       Name = mainMenusFromSession.FirstOrDefault(m => m.Id == g.Key)?.Name,
                       Icon = mainMenusFromSession.FirstOrDefault(m => m.Id == g.Key)?.Icon,
                       SubMenus = g.Select(submenu => new SubMenuViewModel
                       {
                           Id = submenu.Id,
                           Name = submenu.Name,
                           Controller = submenu.Controller,
                           Action = submenu.Action,
                           MainMenuId = submenu.MainMenuId,
                           MainMenuName = mainMenusFromSession.FirstOrDefault(m => m.Id == submenu.MainMenuId)?.Name,
                           RoleId = submenu.RoleId,
                           IsActive = submenu.IsActive
                       }).ToList()
                   })
                   .ToList();

                var jsonSettingsForSession = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var jsonStringFromSession = JsonConvert.SerializeObject(groupedSubMenusWithMainMenuFromSession, jsonSettingsForSession);

                var jsonFoUserDetailsFromSession = JsonConvert.SerializeObject(employees);
                HttpContext.Session.SetString("GroupByUserDetails", jsonFoUserDetailsFromSession);
                HttpContext.Session.SetString("GroupedSubMenus", jsonStringFromSession);

                return RedirectToAction("Dashboard", "Dashboard");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Helper method to store token in both cookie and session
        private void StoreToken(string jwtToken)
        {
            // Store token in both cookie and session
            HttpContext.Response.Cookies.Append("JwtToken", jwtToken, new CookieOptions
            {
                HttpOnly = true, // Prevent access from client-side scripts
                Secure = true,  // Use HTTPS in production
                SameSite = SameSiteMode.Strict
            });

            // Store JWT token in session
            HttpContext.Session.SetString("JwtToken", jwtToken);
        }


        //public async Task<IActionResult> Otp(OtpViewModel model)
        //{
        //    try
        //    {
        //        // Get MagicOTP key from configuration
        //        var magicOtpConfig = _configuration.GetSection("MagicOTP");
        //        bool key = magicOtpConfig.GetValue<bool>("key");
        //        string magicOTP = magicOtpConfig.GetValue<string>("Otp");

        //        // If MagicOTP.key is true, skip OTP validation and proceed with login
        //        if (key)
        //        {
        //            if (model.Otp != magicOTP)
        //            {
        //                TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //                TempData["ToastMessage"] = "Invalid Magic OTP. Please try again.";
        //                return RedirectToAction("Otp");
        //            }

        //            var jwtToken = HttpContext.Session.GetString("JwtToken");
        //            if (string.IsNullOrEmpty(jwtToken))
        //            {
        //                TempData["ToastType"] = "danger";  // Success, danger, warning, info
        //                TempData["ToastMessage"] = "Session expired. Please log in again.";
        //                return RedirectToAction("Login");
        //            }

        //            var (userIds, userNames, emails, roleId) = JwtSessionHelper.ExtractSessionData(jwtToken);

        //            var employee = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == emails && e.Active == true);
        //            if (employee == null || !employee.Active)
        //            {
        //                TempData["ToastType"] = "danger";  // Success, danger, warning, info
        //                TempData["ToastMessage"] = "Employee not found or inactive. Please contact support.";
        //                return RedirectToAction("Login");
        //            }

        //            // Skip OTP logic, proceed to setting session and menu
        //            var userDetails = JwtSessionHelper.ExtractSessionData(jwtToken);

        //            var subMenus = await _context.SubMenus
        //                 .Where(x => x.RoleId == roleId && x.EmpId == employee.EmpId)
        //                 .ToListAsync();

        //            var mainMenuIds = subMenus.Select(x => x.MainMenuId).Distinct().ToList();
        //            var mainMenus = await _context.MainMenus.Where(m => mainMenuIds.Contains(m.Id)).ToListAsync();

        //            var groupedSubMenusWithMainMenu = subMenus
        //               .GroupBy(x => x.MainMenuId)
        //               .Select(g => new MainMenuViewModel
        //               {
        //                   Id = g.Key ?? 0,
        //                   Name = mainMenus.FirstOrDefault(m => m.Id == g.Key)?.Name,
        //                   Icon = mainMenus.FirstOrDefault(m => m.Id == g.Key)?.Icon,
        //                   SubMenus = g.Select(submenu => new SubMenuViewModel
        //                   {
        //                       Id = submenu.Id,
        //                       Name = submenu.Name,
        //                       Controller = submenu.Controller,
        //                       Action = submenu.Action,
        //                       MainMenuId = submenu.MainMenuId,
        //                       MainMenuName = mainMenus.FirstOrDefault(m => m.Id == submenu.MainMenuId)?.Name,
        //                       RoleId = submenu.RoleId,
        //                       IsActive = submenu.IsActive
        //                   }).ToList()
        //               })
        //               .ToList();

        //            var jsonSettings = new JsonSerializerSettings
        //            {
        //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //            };
        //            var jsonString = JsonConvert.SerializeObject(groupedSubMenusWithMainMenu, jsonSettings);

        //            var jsonFoUserDetails = JsonConvert.SerializeObject(userDetails);
        //            HttpContext.Session.SetString("GroupByUserDetails", jsonFoUserDetails);
        //            HttpContext.Session.SetString("GroupedSubMenus", jsonString);

        //            return RedirectToAction("Dashboard", "Dashboard");
        //        }

        //        // OTP handling when MagicOTP.key is not enabled
        //        var otpExpiry = HttpContext.Session.GetString("OtpExpiry");
        //        if (otpExpiry != null && DateTime.TryParse(otpExpiry, out DateTime expiry) && expiry < DateTime.Now)
        //        {
        //            TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Your OTP has expired. Please request a new one.";
        //            return RedirectToAction("Login");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Please enter a valid OTP.";
        //            return RedirectToAction("Otp");
        //        }

        //        var storedOtp = HttpContext.Session.GetString("Otp");

        //        if (string.IsNullOrEmpty(storedOtp))
        //        {
        //            TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "OTP is not available. Please request a new one.";
        //            return RedirectToAction("Otp");
        //        }

        //        if (model.Otp != storedOtp)
        //        {
        //            TempData["ToastType"] = "warning";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Invalid OTP. Please try again.";
        //            return RedirectToAction("Otp");
        //        }

        //        var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
        //        if (string.IsNullOrEmpty(jwtTokenFromSession))
        //        {
        //            TempData["ToastType"] = "danger";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Session expired. Please log in again.";
        //            return RedirectToAction("Login");
        //        }

        //        var (userId, userName, email, roleIdFromSession) = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

        //        var employeeFromSession = await _context.EmployeesCreds.FirstOrDefaultAsync(e => e.Email == email && e.Active == true);
        //        if (employeeFromSession == null || !employeeFromSession.Active)
        //        {
        //            TempData["ToastType"] = "danger";  // Success, danger, warning, info
        //            TempData["ToastMessage"] = "Employee not found or inactive. Please contact support.";
        //            return RedirectToAction("Login");
        //        }

        //        HttpContext.Session.Remove("Otp");
        //        var userDetailsFromSession = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

        //        var subMenusFromSession = await _context.SubMenus
        //             .Where(x => x.RoleId == roleIdFromSession && x.EmpId == employeeFromSession.EmpId)
        //             .ToListAsync();

        //        var mainMenuIdsFromSession = subMenusFromSession.Select(x => x.MainMenuId).Distinct().ToList();
        //        var mainMenusFromSession = await _context.MainMenus.Where(m => mainMenuIdsFromSession.Contains(m.Id)).ToListAsync();

        //        var groupedSubMenusWithMainMenuFromSession = subMenusFromSession
        //           .GroupBy(x => x.MainMenuId)
        //           .Select(g => new MainMenuViewModel
        //           {
        //               Id = g.Key ?? 0,
        //               Name = mainMenusFromSession.FirstOrDefault(m => m.Id == g.Key)?.Name,
        //               Icon = mainMenusFromSession.FirstOrDefault(m => m.Id == g.Key)?.Icon,
        //               SubMenus = g.Select(submenu => new SubMenuViewModel
        //               {
        //                   Id = submenu.Id,
        //                   Name = submenu.Name,
        //                   Controller = submenu.Controller,
        //                   Action = submenu.Action,
        //                   MainMenuId = submenu.MainMenuId,
        //                   MainMenuName = mainMenusFromSession.FirstOrDefault(m => m.Id == submenu.MainMenuId)?.Name,
        //                   RoleId = submenu.RoleId,
        //                   IsActive = submenu.IsActive
        //               }).ToList()
        //           })
        //           .ToList();

        //        var jsonSettingsForSession = new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        };
        //        var jsonStringFromSession = JsonConvert.SerializeObject(groupedSubMenusWithMainMenuFromSession, jsonSettingsForSession);

        //        var jsonFoUserDetailsFromSession = JsonConvert.SerializeObject(userDetailsFromSession);
        //        HttpContext.Session.SetString("GroupByUserDetails", jsonFoUserDetailsFromSession);
        //        HttpContext.Session.SetString("GroupedSubMenus", jsonStringFromSession);

        //        return RedirectToAction("Dashboard", "Dashboard");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
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

            // Clear the JWT token from cookies (if any)
            if (HttpContext.Request.Cookies.ContainsKey("JwtToken"))
            {
                HttpContext.Response.Cookies.Delete("JwtToken");
            }

            // Clear other session data if necessary
            HttpContext.Session.Clear();

            // Redirect the user to the login page or another appropriate page
            return RedirectToAction("Login", "Account");
        }
    }
}


