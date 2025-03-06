using EHRM.Infrastructure.Middleware;
using EHRM.ServiceLayer.Dashboard;
using EHRM.ServiceLayer.Helpers;
using EHRM.ViewModel.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{

   
    public class DashboardController : Controller
    {
        private  IdashboardService _dashboard;
            
        public DashboardController(IdashboardService dashboard)
        {
            _dashboard = dashboard;
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

                if(roleId == 1)
                {
                    ViewData["roleId"] = roleId;
                    List<EmployeeViewModel> adminPageEmployees = _dashboard.GetAllEmployeeDataForAdmin();

                    return View(adminPageEmployees);

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

    }
}
