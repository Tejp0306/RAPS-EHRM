using EHRM.Infrastructure.Middleware;
using EHRM.ServiceLayer.Dashboard;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.Utility;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.PunchDeatils;
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
            if (string.IsNullOrEmpty(userSession))
            {
                return RedirectToAction("Login");
            }

            try
            {
                var userDetails = JwtSessionHelper.ExtractSessionData(userSession);

                if (string.IsNullOrEmpty(userSession))
                {
                    return RedirectToAction("Login");
                }

                var roleId = userDetails.roleId;
                var empId = userDetails.userId; // This is the ManagerId

                if (roleId == 4)
                {
                    if (!int.TryParse(empId, out int managerId))
                    {
                        // Log error if parsing fails
                        throw new Exception("Invalid Manager ID.");
                    }

                    ViewData["roleId"] = roleId;
                    ViewData["empId"] = managerId;

                    List<EmployeeViewModel> employees = _dashboard.GetEmployeesByManager(managerId);

                    return View(employees);
                }

                if (roleId == 1)
                {
                    ViewData["roleId"] = roleId;
                    List<EmployeeViewModel> adminPageEmployees = _dashboard.GetAllEmployeeDataForAdmin();
                    EmployeeViewModel employeeViewModel = new EmployeeViewModel();

                    return View(adminPageEmployees);
                    //return View("_PunchDetailPartialView", adminPageEmployees[0].PunchDetail);
                }


                else
                {

                    var userId = userDetails.userId;
                    if (!int.TryParse(userId, out int userdashboardid))
                    {
                        // Log error if parsing fails
                        throw new Exception("Invalid Manager ID.");
                    }

                    
                    ViewData["empId"] = userdashboardid;

                    List<EmployeeViewModel> userEmployee = _dashboard.GetDataForUserDashboard(userdashboardid);

                    //EmployeeViewModel userEmployee = _dashboard.GetDataForUserDashboard(userdashboardid).FirstOrDefault();

                    return View(userEmployee);

                }


            }
            catch (Exception ex)
            {
                // Log the error (use a logging framework like Serilog or NLog)
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
                         RecipentMail = "arjunsingh@rapscorp.com",  // Replace with actual recipient email
                         CcMail = "saksham@rapscorp.com",  // Replace with actual CC email
                         Subject = "Punched In",  // Subject for Punch In
                                                  // Include the user name and the current time in the email body for personalization
                         Body = $"{userName} has punched in successfully at {currentTime}. Thanks."  // Personalized Body for Punch In
                     };

                      _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);
                     Response.Cookies.Append("PunchRes", "True");
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
            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Call the service method to update the role
                //var result =  _accountService.UpdatePunchOutAsync(EmpId);
                var result =await _dashboard.UpdatePunchOutAsync(EmpId);
                if (result.Success) {
                //if (result.IsCompletedSuccessfully)
                //{
                    // Success handling for create
                    //TempData["ToastType"] = "success";
                    //TempData["ToastMessage"] = "Punch-Out saved successfully!";
                    
                    // Prepare email content for Punch Out
                    EmailServiceModel _email = new()
                    {
                        RecipentMail = "arjunsingh@rapscorp.com",  // Replace with actual recipient email
                        CcMail = "saksham@rapscorp.com",  // Replace with actual CC email
                        Subject = "Punched Out",  // Subject for Punch Out
                        Body = $"{userName} has punched out successfully at {currentTime}. Thanks."  // Personalized Body for Punch Out
                    };

                    // Sending the email for Punch Out
                    _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);
                    Response.Cookies.Delete("PunchRes");
                    return RedirectToAction("Dashboard");
                    //return Json(new { success = true }); 
                }
                else
                {
                    // Error handling for create failure
                    //TempData["ToastType"] = "danger";
                    //TempData["ToastMessage"] = "Punch-Out saved successfully!";
                    return RedirectToAction("Dashboard");
                    //return Json(new { success = false });// Redirect to the same page in case of error
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Unexpected error occurred: {ex.Message}";
                //return RedirectToAction("Dashboard");
                return Json(new { success = false });
            }
        }

    }
}
