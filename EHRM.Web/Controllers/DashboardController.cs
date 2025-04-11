using System.Security.Principal;
using EHRM.Infrastructure.Middleware;
using EHRM.ServiceLayer.Dashboard;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.Utility;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.PunchDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{

    public class DashboardController : Controller
    {
        private  IdashboardService _dashboard;
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        public DashboardController(IdashboardService dashboard, IAccountService accountService, IEmailService emailService)
        {
            _dashboard = dashboard;
            _accountService = accountService;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Dashboard()
        {
            var userSession = HttpContext.Session.GetString("JwtToken");

            // If there is no JWT token, redirect to login
            if (string.IsNullOrEmpty(userSession))
            {
                return RedirectToAction("Login");
            }

            try
            {
                var userDetails = JwtSessionHelper.ExtractSessionData(userSession);

                // If there is still no JWT token in the session, redirect to login
                if (string.IsNullOrEmpty(userSession))
                {
                    return RedirectToAction("Login");
                }

                var roleId = userDetails.roleId;
                var empId = userDetails.userId; 
                var punchResCookie = HttpContext.Request.Cookies["PunchRes"];
                if (punchResCookie != null && punchResCookie != empId.ToString())
                {
                    // If the EmpId in the session does not match the one in the PunchRes cookie, delete the cookie
                    Response.Cookies.Delete("PunchRes");

                }

                // Logic for Manager (Role 4)
                if (roleId == 4)
                {
                    if (!int.TryParse(empId, out int managerId))
                    {
                        throw new Exception("Invalid Manager ID.");
                    }

                    ViewData["roleId"] = roleId;
                    ViewData["empId"] = managerId;

                    List<EmployeeViewModel> employees = _dashboard.GetEmployeesByManager(managerId);

                    return View(employees);
                }

                // Logic for Admin (Role 1)
                if (roleId == 1)
                {
                    ViewData["roleId"] = roleId;
                    List<EmployeeViewModel> adminPageEmployees = _dashboard.GetAllEmployeeDataForAdmin();

                    return View(adminPageEmployees);
                }

                // Logic for other roles (e.g., Regular Employee)
                else
                {
                    if (!int.TryParse(empId, out int userdashboardid))
                    {
                        throw new Exception("Invalid User ID.");
                    }

                    ViewData["empId"] = userdashboardid;

                    List<EmployeeViewModel> userEmployee = _dashboard.GetDataForUserDashboard(userdashboardid);

                    return View(userEmployee);
                }
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error: {ex.Message}");

                // Redirect to an error page or return an error view
                return View("Error");
            }
        }


        [HttpGet("Dashboard/SavePunchInAsync")]
        public async Task<IActionResult> SavePunchInAsync()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            int EmpId = Convert.ToInt32(userDetails.userId);
            var userName = userDetails.userName;
            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                 var result = await _dashboard.SavePunchInAsync(EmpId, userName);

                 if (result.Success)
                 {
                     // Success handling for create
                     //TempData["ToastType"] = "success";
                     //TempData["ToastMessage"] = "Punch-In saved successfully!";
                     EmailServiceModel _email = new()
                     {
                         RecipentMail = "saksham@rapscorp.com",  // Replace with actual recipient email
                         CcMail = "saksham@rapscorp.com",  // Replace with actual CC email
                         Subject = "Punched In",  // Subject for Punch In
                                                  // Include the user name and the current time in the email body for personalization
                         Body = $"{userName} has punched in successfully at {currentTime}. Thanks."  // Personalized Body for Punch In
                     };

                      _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);
                     Response.Cookies.Append("PunchRes", EmpId.ToString());
                     return RedirectToAction("Dashboard"); // Redirect to the dashboard or another page
                 }
                 else
                 {
                     // Error handling for create failure
                     //TempData["ToastType"] = "danger";
                     //TempData["ToastMessage"] = result.Message;
                     Response.Cookies.Delete("PunchRes");

                     return RedirectToAction("Dashboard"); // Redirect to the same page in case of error
                 }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Unexpected error occurred: {ex.Message}";
                return RedirectToAction("Dashboard");
            }
        }



        [HttpGet("Dashboard/UpdatePunchOutAsync")]
        public async Task<IActionResult> UpdatePunchOutAsync()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            int EmpId = Convert.ToInt32(userDetails.userId);
            var userName = userDetails.userName;
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            try
            {
                // Call the service method to update the role
                var result = await _dashboard.UpdatePunchOutAsync(EmpId);

                if (result.Success)
                {
                    // Prepare email content for Punch Out
                    EmailServiceModel _email = new()
                    {
                        RecipentMail = "saksham@rapscorp.com",  // Replace with actual recipient email
                        CcMail = "saksham@rapscorp.com",  // Replace with actual CC email
                        Subject = "Punched Out",  // Subject for Punch Out
                        Body = $"{userName} has punched out successfully at {currentTime}. Thanks."  // Personalized Body for Punch Out
                    };

                    // Sending the email for Punch Out
                     _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);

                    // Remove "PunchRes" cookie (if needed)
                    Response.Cookies.Delete("PunchRes");


                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(1),  
                        HttpOnly = true
                    };
                    Response.Cookies.Append("PunchCooldown", "true", options);

                    // Redirect to Dashboard after successful punch-out
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    // Error handling if punch-out fails
                    return RedirectToAction("Dashboard");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Unexpected error occurred: {ex.Message}";
                return Json(new { success = false });
            }
        }


    }
}
