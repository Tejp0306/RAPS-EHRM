using EHRM.DAL.Database;
using EHRM.ServiceLayer.ExitFormalities;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.PostJoining;
using EHRM.ViewModel.ExitFormalities;
using EHRM.ViewModel.PostJoining;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace EHRM.Web.Controllers
{
    public class ExitFormalitiesController : Controller
    {
        private readonly IExitFormalitiesService _exit;

        public ExitFormalitiesController(IExitFormalitiesService exit)
        {

            _exit = exit;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Exit Interview

        public IActionResult ExitInterview()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult ExitInterviewDetails()
        {

            return View();
        }


        public async Task<IActionResult> SaveExitInterviewForm(ExitInterviewViewModel model)
        {


            var result = await _exit.CreateExitInterviewFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("ExitInterview"); // Redirect back to the EmployeeType view

            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("ExitInterview"); // Redirect back to the EmployeeType view
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetExitInterviewForm()
        {
            // Fetch the result from the service layer
            var result = await _exit.GetExitInterviewFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<ExitInterviewForm>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        InterviewDate = asset.InterviewDate,

                    }).ToList();

                    return Json(AssetList);
                }

                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No Asset found." });
            }
        }

        //[HttpGet("ExitFormalities/GetExitInterviewFormDetails/{ID}")]
        public async Task<JsonResult> GetExitInterviewFormDetails([FromRoute] int Id)
        {
            try
            {
                var asset = await _exit.GetExitInterviewFormByIdAsync(Id);

                if (asset == null)
                {
                    return Json(new { success = false, message = "Asset not found." });
                }

                return Json(new { success = true, data = asset });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Asset details." });
            }
        }


        #endregion


        #region Resignation Form

        public IActionResult ResignationForm()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult ResignationFormDetails()
        {

            return View();
        }


        public async Task<IActionResult> SaveResignationForm(ResignationFormViewModel model)
        {
            var result = await _exit.CreateResignationFormAsync(model);

            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("ResignationForm"); // Redirect back to the EmployeeType view

            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("ResignationForm"); // Redirect back to the EmployeeType view
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetResignationForm()
        {
            // Fetch the result from the service layer
            var result = await _exit.GetResignationFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<ResignationForm>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        Position = asset.Position,

                    }).ToList();

                    return Json(AssetList);
                }

                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No Asset found." });
            }
        }

        public async Task<JsonResult> GetResignationFormById([FromRoute] int Id)
        {
            try
            {
                var asset = await _exit.GetResignationFormByIdAsync(Id);

                if (asset == null)
                {
                    return Json(new { success = false, message = "Asset not found." });
                }

                return Json(new { success = true, data = asset });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Asset details." });
            }
        }


        #endregion

        #region Employee Undertaking

        public IActionResult EmployeeUndertaking()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult EmployeeUndertakingDetail()
        {
            return View();
        }

        public async Task<IActionResult> SaveEmployeeUndertakingForm(EmployeeUndertakingViewModel model)
        {
            var result = await _exit.CreateEmployeeUndertakingFormAsync(model);

            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("EmployeeUndertaking"); 

            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("EmployeeUndertaking"); 
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetEmployeeUndertaking()
        {
            // Fetch the result from the service layer
            var result = await _exit.GetEmpUndertakingFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<EmployeeUndertakingForm>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        LastWorkingDate = asset.LastWorkingDate,

                    }).ToList();

                    return Json(AssetList);
                }

                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No Asset found." });
            }
        }

        public async Task<JsonResult> GetEmployeeUndertakingById([FromRoute] int Id)
        {
            try
            {
                var asset = await _exit.GetEmpUndertakingFormByIdAsync(Id);

                if (asset == null)
                {
                    return Json(new { success = false, message = "Asset not found." });
                }

                return Json(new { success = true, data = asset });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Asset details." });
            }
        }

        #endregion


        #region  Employee Exit Checklist

        public IActionResult ExitChecklist()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            var empid = userDetails.userId;
            ViewData["Name"] = name;
            ViewData["EmpId"] = Convert.ToInt32(userDetails.userId); ;
            return View();
        }

        public IActionResult ExitChecklistDetails()
        {

            return View();
        }

        public async Task<IActionResult> SaveCkecklistForm(ExitChecklistViewModel model)
        {


            var result = await _exit.CreateExitChecklistFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("ExitChecklist"); // Redirect back to the EmployeeType view

            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("ExitChecklist"); // Redirect back to the EmployeeType view
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetExitCkecklist()
        {
            // Fetch the result from the service layer
            var result = await _exit.GetExitChecklistFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<EmployeeExitChecklist>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        Name = asset.Name,
                        ReportingManager = asset.ReportingManager,

                    }).ToList();

                    return Json(AssetList);
                }

                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No Asset found." });
            }
        }

        public async Task<JsonResult> GetExitCkecklistById([FromRoute] int Id)
        {
            try
            {
                var asset = await _exit.GetExitChecklistFormByIdAsync(Id);

                if (asset == null)
                {
                    return Json(new { success = false, message = "Asset not found." });
                }

                return Json(new { success = true, data = asset });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Asset details." });
            }
        }



        #endregion

    }


}
