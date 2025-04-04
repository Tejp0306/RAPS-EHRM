using EHRM.ViewModel.MasterEmployee;
using EHRM.ViewModel.MainMenu;
using Microsoft.AspNetCore.Mvc;
using EHRM.ServiceLayer.PostJoining;
using EHRM.ViewModel.Employee;
using Microsoft.EntityFrameworkCore;
using EHRM.DAL.Database;
using EHRM.ServiceLayer.Helpers;
using NuGet.Protocol;
using Newtonsoft.Json.Linq;

namespace EHRM.Web.Controllers
{
    public class PostJoiningController : Controller
    {
        private readonly IPostJoiningService _post;

        public PostJoiningController(IPostJoiningService postJoiningService)
        {
            _post = postJoiningService;
        }
        public IActionResult MasterSheet()
        {
            return View();
        }

        public IActionResult BGVForm()
        {
            return View();
        }

        public IActionResult PostJoiningForms()
        {
            return View();
        }

        public IActionResult AdminBGVView()
        {
            return View();
        }

        public IActionResult AdminMasterSheetView()
        {
            return View();
        }

        public IActionResult AddExperience()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var userId = userDetails.userId;
            ViewData["userId"] = userId;
            return View();
        }


        // Save Personal Info of MasterSheet
        [HttpPost]
        public async Task<IActionResult> SavePersonalMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterEmployee.AadharNumber is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterSheetAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Personal Info saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }

        // Save Contact Info of MasterSheet

        public async Task<IActionResult> SaveContactMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterContactDetails.PersonalContactNo is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterContactAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Contact saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }


        // Save Address Info of MasterSheet

        public async Task<IActionResult> SaveAddressMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterAddress.PermanentAddress is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterAddressAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }

        // Save Education Info of MasterSheet

        public async Task<IActionResult> SaveEducationMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterEducation.XthInstitution is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterEducationAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }

        // Save Experience Info of MasterSheet

        public async Task<IActionResult> SaveExperienceMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterWorkExperience[0].OrganisationName is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterExperienceAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }


        // Save Bank Info of MasterSheet
        public async Task<IActionResult> SaveBankMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterBankDetails.BankName is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterBankAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }

        // Save Emergency Info of MasterSheet

        public async Task<IActionResult> SaveEmergencyMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterEmergencyContactViewModel.EmergencyContactNumber is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterEmergencyAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }



        // Save Reporting Info of MasterSheet

        public async Task<IActionResult> SaveReportingMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterReportingDetails.DirectReporting is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterReportingAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }



        // Save Family Info of MasterSheet

        public async Task<IActionResult> SaveFamilyMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterFamilyDetails.RelationWithEmployee is null)
            {
                // Return to the form if validation fails
                return Redirect("MasterSheet");
            }

            var result = await _post.SaveMasterFamilyAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }




        // Save Dependent Info of MasterSheet

        public async Task<IActionResult> SaveDependentMasterInfo(EmployeeFormViewModel model)
        {
            if (model.MasterDependentDetails.DependentName is null)
            {
                // Return to the form if validation fails
                return View(model);
            }

            var result = await _post.SaveMasterDependentAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Master Address saved successfully.";
                return Redirect("MasterSheet");

            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }




        [HttpPost]
        public async Task<IActionResult> SaveBGVForm(BGVViewModel model)
        {
            if (model.Email is null)
            {
                // Return to the form if validation fails
                return View(model);
            }

            var result = await _post.SaveBGVFormAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "BGV Form saved successfully.";
                return RedirectToAction("AddExperience");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> SavePreviousEmployments(EmploymentViewModel model)
        {

            if (model.PreviousEmployments is null)
            {
                // Return to the form if validation fails
                return View(model);
            }

            var result = await _post.SavePreviousEmploymentsAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "BGV Form saved successfully.";
                return RedirectToAction("AddExperience");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetBGVDetails(int EmpId)
        {
            try
            {
                var nb = await _post.GetEmployeeDetailsAsync(EmpId);

                if (nb == null)
                {
                    return Json(new { success = false, message = "Notice not found." });
                }


                return Json(new { success = true, data = nb });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the notice details." });
            }

        }



        [HttpGet]
        public async Task<IActionResult> GetMasterSheetData(int EmpId)
        {
            try
            {
                EmployeeFormViewModel employeeData = await Task.Run(() => _post.GetMasterSheetDataAsync(EmpId));

                if (employeeData == null)
                {
                    return Json(new { success = false, message = "Employee data not found." });
                }

                return Json(new { success = true, data = employeeData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the employee data.", error = ex.Message });
            }
        }



        //Get All BGV and MasterSheet Data for Admin

        [HttpGet]
        public async Task<JsonResult> GetBackGroundFormDetails()
        {
            // Fetch the result from the service layer
            var result = await _post.GetBackGroundFormDetailsAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var data = result.Data; // Do not cast to dynamic

                var bgvForms = data.GetType().GetProperty("BgvForms")?.GetValue(data) as IEnumerable<Bgvform>;
                var previousEmployments = data.GetType().GetProperty("PreviousEmployments")?.GetValue(data) as IEnumerable<PreviousEmployment>;

                // Ensure lists are not null
                bgvForms ??= new List<Bgvform>();
                previousEmployments ??= new List<PreviousEmployment>();

                var response = new
                {
                    BgvForms = bgvForms.Select(bgv => new
                    {
                        EmpId = bgv.EmpId,
                        EmployeeName = bgv.FirstName + " " + bgv.LastName,
                        EmailAddress = bgv.Email
                    }).ToList(),


                };

                return Json(new { Success = true, Data = response });
            }

            return Json(new { Success = false, Message = result.Message ?? "No background form details found." });
        }


        [HttpGet]
        public async Task<JsonResult> GetMasterSheetFormDetails()
        {
            try
            {
                var result = await _post.GetMasterSheetFormDetailsAsync();

                if (result?.Success == true && result.Data != null)
                {
                    var jsonData = JObject.FromObject(result.Data);

                    // Extract MasterForm instead of EmployeeMaster
                    var employeeMasterList = jsonData["MasterForm"]?.ToObject<List<EmployeeMaster>>() ?? new List<EmployeeMaster>();

                    var response = new
                    {
                        BgvForms = employeeMasterList.Select(emp => new
                        {
                            EmpId = emp.EmpId,
                            EmployeeName = $"{emp.FirstName} {emp.LastName}",
                            EmailAddress = emp.PancardNumber // Change if you need a different field
                        }).ToList()
                    };

                    return Json(new { Success = true, Data = response });
                }

                return Json(new { Success = false, Message = result?.Message ?? "No background form details found." });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "An error occurred while retrieving background form details.", Error = ex.Message });
            }
        }







        [HttpGet("PostJoining/GetBGVDataByEmpId/{EmpId}")]
        public async Task<JsonResult> GetBGVDataByEmpId([FromRoute] int EmpId)
        {
            try
            {
                var nb = await _post.GetEmployeeDetailsAsync(EmpId);

                if (nb == null)
                {
                    return Json(new { success = false, message = "Notice not found." });
                }


                return Json(new { success = true, data = nb });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the notice details." });
            }

        }

        // Get MasterSheet Data for Admin

        [HttpGet("PostJoining/GetMasterSheetDataByEmpId/{EmpId}")]
        public async Task<JsonResult> GetMasterSheetDataByEmpId([FromRoute] int EmpId)
        {
            try
            {
                EmployeeFormViewModel employeeMasterData = await Task.Run(() => _post.GetMasterSheetDataAsync(EmpId));

                if (employeeMasterData == null)
                {
                    return Json(new { success = false, message = "Employee data not found." });
                }

                return Json(new { success = true, data = employeeMasterData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the employee data.", error = ex.Message });
            }

        }


    }
}
