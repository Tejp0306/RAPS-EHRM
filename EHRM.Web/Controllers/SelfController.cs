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
        private readonly IConfiguration _configuration;
        public SelfController(ISelfService Self, IConfiguration configuration)
        {

            _self = Self;
            _configuration = configuration;
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

        [HttpGet("Self/EmployeeData/{EmpId?}")]
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
                    if (model.DailyEntries == null || !model.DailyEntries.Any())
                    {
                        return Json(new { success = false, message = "No daily entries found." });
                    }

                    // Set the EmpId from the session data
                    model.EmpId = Convert.ToInt32(empId);

                    // Call the service method to save the timesheet
                    var result = await _self.CreateTimeSheetAsync(model, model.FilePath);

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
                    return Json(new { success = false, message = "Invalid timesheet data." });
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

        [HttpPost]
        public JsonResult UploadFiles(List<IFormFile> Files)
        {
            try
            {
                if (Files == null || Files.Count == 0)
                {
                    return Json(new { success = false, message = "No files uploaded." });
                }

                List<string> filePaths = new List<string>();
                string path;
                if (_configuration["AppSetting:EnvironmentName"].ToString().Equals("Production"))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "Files");
                }
                else
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
                }
                // Corrected path (No extra backslashes)
               



                // Ensure the directory exists
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var file in Files)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    string filePath = Path.Combine(path, fileName); // Corrected file save path

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Store the file path in the correct format: \Files\filename.ext
                    string storedPath = Path.Combine("Files", fileName);  // Removed leading backslash
                    filePaths.Add("//" + storedPath); // Ensure correct format for output
                }

                return Json(new { success = true, filePaths });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "File upload failed: " + ex.Message });
            }
        }

        //[HttpPost]
        //public JsonResult UploadFiles(List<IFormFile> Files)
        //{
        //    try
        //    {
        //        // Check if files are provided
        //        if (Files == null || Files.Count == 0)
        //        {
        //            return Json(new { success = false, message = "No files uploaded." });
        //        }

        //        List<string> filePaths = new List<string>();

        //        // Define the directory path to store files
        //        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");

        //        // Create the folder if it doesn't exist
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        foreach (var file in Files)
        //        {
        //            // Generate a unique file name
        //            FileInfo fileInfo = new FileInfo(file.FileName);
        //            string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;

        //            // Combine path with file name
        //            string fileNameWithPath = Path.Combine(path, fileName);

        //            // Save the file
        //            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }

        //            // Store the relative path for client access
        //            string storedPath = Path.Combine("Files", fileName);
        //            filePaths.Add("/" + storedPath.Replace("\\", "/")); // Ensure forward slashes for URL
        //        }

        //        return Json(new { success = true, filePaths });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "File upload failed: " + ex.Message });
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> ShowFile(int id)
        {
            try
            {
                var nb = await _self.GetFilesAsync(id);

                if (nb == null)
                {
                    return Json(new { success = false, message = "Attachments not found." });
                }


                return Json(new { success = true, data = nb });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Attachments." });
            }
        }





        #endregion
    }
}
