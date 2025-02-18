using EHRM.DAL.Database;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.Self;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Review;
using EHRM.ViewModel.Self;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
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
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
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



        #region TimeSheet
        public IActionResult TimeSheetDetails()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SubmitTimeSheet([FromBody] TimeSheetViewModel model)
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;

            try
            {
                if (ModelState.IsValid)
                {
                    // Ensure the DailyEntries are populated
                    if (model.DailyEntries == null || !model.DailyEntries.Any())
                    {
                        return Json(new { success = false, message = "No daily entries found. Please fill in the daily timesheet entries." });
                    }

                    // Set the EmpId from the session data
                    model.EmpId = Convert.ToInt32(empId);

                    // Call the service method to either create or update the timesheet
                    var result = await _self.CreateTimeSheetAsync(model);

                    if (result.Success)
                    {
                        return Json(new { success = true, message = result.Message });
                    }
                    else
                    {
                        return Json(new { success = false, message = result.Message });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Failed to submit timesheet. Please check the data." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error saving timesheet: {ex.Message}" });
            }
        }


        [HttpGet("Self/GetTimesheetForEdit")]
        public async Task<JsonResult> GetTimesheetForEdit()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            int empId = Convert.ToInt32(userDetails.userId);

            var timeSheet = await _self.GetTimeSheetByIdAsync(empId);

            if (timeSheet == null)
            {
                return Json(new { success = false, message = "Timesheet not found." });
            }

            return Json(new { success = true, data = timeSheet });
        }


        [HttpGet]
        public async Task<JsonResult> GetTimeSheetByMonthData(string month)
        {
            // Fetch the result from the service layer, passing the selected month
            var result = await _self.GetTimeSheetByMonthAsync(month);

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var timeSheet = result.Data as IEnumerable<TimeSheet>;
                if (timeSheet != null)
                {
                    var timeSheetList = timeSheet.Select(ts => new
                    {
                        Id = ts.Id,
                        Name = ts.EmpName,
                        Project = ts.ProjectName,
                    }).ToList();

                    return Json(timeSheetList); // Return filtered data as JSON
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                return Json(new { Success = false, Message = result.Message ?? "No timesheet found for this month." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTimeSheetById(int id)
        {
            // Fetch the timesheet data by ID asynchronously
            var timeSheet = await _self.GetTimeSheetsByIdAsync(id);

            if (timeSheet == null)
            {
                return Json(new { success = false, message = "Timesheet not found." });
            }

            return Json(new { success = true, data = timeSheet });
        }


        #endregion
    }
}
