using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.Self;
using EHRM.ViewModel.Employee;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.Design;

namespace EHRM.Web.Controllers
{
    public class SelfController : Controller
    {
        private readonly ISelfService _self;

        public SelfController(ISelfService Self)
        {

            _self = Self;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SelfPortal(int id)
        { 
            var result = await _self.GetAllSelfEmployeeRecordDetails(id);

            if (result.Count > 0)
            {
                TempData["ToastType"] = "success"; // Store success message
                TempData["ToastMessage"] = "User Found";
                var Model = result[0];

                return View(Model);
            }
            else
            {
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "User may deleted or removed";

                return View();
            }
        }

        public IActionResult Profile(EmployeeViewModel model)
        {
            return View(model);
        }

        public async Task<IActionResult> GetSelfProfileDataByIDDOB(EmployeeViewModel model)
        {
            // Fetch the result from the service layer

            //string Parseddob = DateTime.ParseExact(model.DateOfBirth, "dd - MMMM - yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            var datedata = DateTime.Parse(model.DateOfBirth).ToString("dd/MM/yyyy");


            String updateddatedata = datedata.Substring(0, 10);


            var result = await _self.GetDetailsByEmpIdDOB(model.EmpId, updateddatedata);

            if (result.Count > 0)
            {
                TempData["ToastType"] = "success"; // Store success message
                TempData["ToastMessage"] = "User Found";

                EmployeeViewModel employee = new()
                {
                    EmpId = result[0].EmpId,
                    DateOfBirth = result[0].DateOfBirth
                };

                return View("Profile", employee);
            }
            else
            {
                model.EmpId = 0;

                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "User may deleted or removed";

                return View("Self","Profile");
            }
        }
        public IActionResult TimeSheet()
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var userId = userDetails.userId;
            // Pass the userId to the view
            ViewData["UserId"] = userId;
            return View();
        }

        [HttpGet("Employee/EmployeeData/{EmpId?}")]
        public async Task<JsonResult> EmployeeData(int? EmpId = null)
        {
            if (EmpId.HasValue)
            {
                // Fetch employee data based on the provided EmpId
                var res = await _self.GetAllEmployeeDataDetails(EmpId.Value);

                if (res == null || !res.Any())  // Check if no data is returned
                {
                    // If no employee data is found, return a failure response
                    TempData["ErrorMessage"] = "Employee data not found or is in an unexpected format.";
                }

                // Return employee data if found
                return Json(new { Success = true, Data = res });
            }
            else
            {
                // Return an empty list or default message if EmpId is not provided
                var defaultModel = new List<GetAllEmployeeViewModel>();
                return Json(new { Success = true, Data = defaultModel, Message = "No EmpId provided. Returning default data." });
            }
        }


    }
}
