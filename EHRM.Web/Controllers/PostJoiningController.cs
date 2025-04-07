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
﻿using System.Security.Cryptography.Xml;
using EHRM.DAL.Database;
using EHRM.ServiceLayer.Calendar;
using EHRM.ServiceLayer.Document;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.PostJoining;
using EHRM.ServiceLayer.Review;
using EHRM.ServiceLayer.Utility;
using EHRM.ViewModel.Document;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.PostJoining;
using EHRM.ViewModel.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace EHRM.Web.Controllers
{
    public class PostJoiningController : Controller
    {
        private readonly IPostJoiningService _post;

        public PostJoiningController(IPostJoiningService post)
        {
            _post = post;
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
                return Redirect("AddExperience");

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

                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("AcknowledgementForm"); // Redirect back to the EmployeeType view
            }

        }

        [HttpGet("PostJoining/GetAcknowlegementFormDetails/{EmpId}")]
        public async Task<JsonResult> GetAcknowlegementFormDetails([FromRoute] int EmpId)
        {
            try
            {
                var asset = await _post.GetAcknowldegementFormByIdAsync(EmpId);

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

        [HttpGet("PostJoining/GetAcknowlegementDetails/{acknowlegementFormID}")]
        public async Task<JsonResult> GetAcknowlegementDetails([FromRoute] int acknowlegementFormID)
        {
            try
            {
                var asset = await _post.GetAcknowldegementAsync(acknowlegementFormID);

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


        [HttpGet]
        public async Task<JsonResult> GetAllAcknowledgeForm()
        {
            // Retrieve JWT token from session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

            // Get the logged-in employee's name
            var loggedInEmployeeName = userDetails.userName;

            if (string.IsNullOrEmpty(loggedInEmployeeName))
            {
                return Json(new { Success = false, Message = "Session expired or user not logged in." });
            }

            // Fetch all acknowledgment forms
            var result = await _post.GetAllAcknowledgeFormAsync();

            if (result.Success && result.Data != null)
            {
                var allForms = result.Data as IEnumerable<AcknowledgementForm>;

                if (allForms != null)
                {
                    // Filter forms based on the logged-in employee's name
                    var filteredForms = allForms
                        .Where(asset => asset.EmployeeName == loggedInEmployeeName)
                        .Select(asset => new
                        {
                            Id = asset.Id,
                            EmployeeName = asset.EmployeeName,
                            SignatureDate = asset.SignatureDate
                        })
                        .ToList();

                    return Json(filteredForms);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                return Json(new { Success = false, Message = result.Message ?? "No acknowledgment forms found." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAcknowledgeForm()
        {
            // Fetch the result from the service layer
            var result = await _post.GetAcknowledgeFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<AcknowledgementForm>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        SignatureDate = asset.SignatureDate,

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



        #endregion


        #region Personal Information Form

        public IActionResult PersonalInformationForm()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult PersonalInformationDetails()
        {
            return View();
        }

        public async Task<IActionResult> SavePersonalInfoForm(PersonalInfomationViewModel model)
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;
            model.EmpId = Convert.ToInt32(empId);

            var result = await _post.CreatePersonalInformationFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("PersonalInformationForm"); // Redirect back to the EmployeeType view


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

            if (result.Success)
            {
                return Json(result.Data); // Return data as JSON
            }

            // Handle the error scenario
            TempData["ToastType"] = "danger";
            TempData["ToastMessage"] = "An error occurred while retrieving background form details.";

            return Json(new { success = false, message = "Failed to retrieve background form details." });
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




        #region Acknowledgement Form
        public IActionResult AcknowledgementForm()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult AcknowledgementFormDetails()

        {
            return View();
        }


        
        public async Task<IActionResult> SaveAcknowledgementForm(AcknowledgementFormViewModel model)
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;
            model.EmpId = Convert.ToInt32(empId);


            var result = await _post.CreateAcknowldegementFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("AcknowledgementForm"); // Redirect back to the EmployeeType view


            }
            else
            {

                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }

        
        [HttpGet("PostJoining/GetAcknowlegementFormDetails/{EmpId}")]
        public async Task<JsonResult> GetAcknowlegementFormDetails([FromRoute] int EmpId)
        {
            try
            {
                var asset = await _post.GetAcknowldegementFormByIdAsync(EmpId);

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

        [HttpGet("PostJoining/GetAcknowlegementDetails/{acknowlegementFormID}")]
        public async Task<JsonResult> GetAcknowlegementDetails([FromRoute] int acknowlegementFormID)
        {
            try
            {
                var asset = await _post.GetAcknowldegementAsync(acknowlegementFormID);

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


        [HttpGet]
        public async Task<JsonResult> GetAllAcknowledgeForm()
        {
            // Retrieve JWT token from session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

            // Get the logged-in employee's name
            var loggedInEmployeeName = userDetails.userName;

            if (string.IsNullOrEmpty(loggedInEmployeeName))
            {
                return Json(new { Success = false, Message = "Session expired or user not logged in." });
            }

            // Fetch all acknowledgment forms
            var result = await _post.GetAllAcknowledgeFormAsync();

            if (result.Success && result.Data != null)
            {
                var allForms = result.Data as IEnumerable<AcknowledgementForm>;

                if (allForms != null)
                {
                    // Filter forms based on the logged-in employee's name
                    var filteredForms = allForms
                        .Where(asset => asset.EmployeeName == loggedInEmployeeName)
                        .Select(asset => new
                        {
                            Id = asset.Id,
                            EmployeeName = asset.EmployeeName,
                            SignatureDate = asset.SignatureDate
                        })
                        .ToList();

                    return Json(filteredForms);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                return Json(new { Success = false, Message = result.Message ?? "No acknowledgment forms found." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAcknowledgeForm()
        {
            // Fetch the result from the service layer
            var result = await _post.GetAcknowledgeFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<AcknowledgementForm>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        SignatureDate = asset.SignatureDate,

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



        #endregion


        #region Personal Information Form

        public IActionResult PersonalInformationForm()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult PersonalInformationDetails()
        {
            return View();
        }

        public async Task<IActionResult> SavePersonalInfoForm(PersonalInfomationViewModel model)
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;
            model.EmpId = Convert.ToInt32(empId);

            var result = await _post.CreatePersonalInformationFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("PersonalInformationForm"); // Redirect back to the EmployeeType view


            }
            else
            {

                TempData["ErrorMessage"] = "An error occurred while saving data.";
                return View(model);
            }
        }



        
        [HttpGet]
        public async Task<JsonResult> GetAllPersonalInfoForm()
        {
            // Retrieve JWT token from session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

            // Get the logged-in employee's name
            var loggedInEmployeeName = userDetails.userName;

            if (string.IsNullOrEmpty(loggedInEmployeeName))
            {
                return Json(new { Success = false, Message = "Session expired or user not logged in." });
            }

            // Fetch all acknowledgment forms
            var result = await _post.GetAllPersonalInformationFormAsync();

            if (result.Success && result.Data != null)
            {
                var allForms = result.Data as IEnumerable<PersonalInfo>;

                if (allForms != null)
                {
                    // Filter forms based on the logged-in employee's name
                    var filteredForms = allForms
                        .Where(asset => asset.EmployeeName == loggedInEmployeeName)
                        .Select(asset => new
                        {
                            Id = asset.Id,
                            EmployeeName = asset.EmployeeName,
                            PersonalEmail = asset.PersonalEmail,
                        })
                        .ToList();

                    return Json(filteredForms);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                return Json(new { Success = false, Message = result.Message ?? "No acknowledgment forms found." });
            }
        }

        [HttpGet("PostJoining/GetPersonalInfoById/{EmpId}")]
        public async Task<JsonResult> GetPersonalInfoById([FromRoute] int EmpId)
        {
            try
            {
                var asset = await _post.GetPersonalInfoByIdAsync(EmpId);

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


        [HttpGet("PostJoining/GetPersInfoById/{personalInfoID}")]
        public async Task<JsonResult> GetPersInfoById([FromRoute] int personalInfoID)
        {
            try
            {
                var asset = await _post.GetPersonalInfoAsync(personalInfoID);

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

        [HttpGet]
        public async Task<JsonResult> GetPersInfoForm()
        {
            // Fetch the result from the service layer
            var result = await _post.GetPersonalInfoFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<PersonalInfo>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        PersonalEmail = asset.PersonalEmail,

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

        #endregion


        #region Client Property Declaration

        public IActionResult ClientDeclarationForm()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult ClientDeclarationFormDetails()
        {

            return View();
        }


        public async Task<IActionResult> SaveClientPropertyDeclarationForm(ClientPropertyDeclarationViewModel model)
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;
            model.EmpId = Convert.ToInt32(empId);

            var result = await _post.CreateClientPropertDeclarationFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("ClientDeclarationForm"); // Redirect back to the EmployeeType view

            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("ClientDeclarationForm"); // Redirect back to the EmployeeType view
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetAllClientPropertyDeclaration()
        {
            // Retrieve JWT token from session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

            // Get the logged-in employee's name
            var loggedInEmployeeName = userDetails.userName;

            if (string.IsNullOrEmpty(loggedInEmployeeName))
            {
                return Json(new { Success = false, Message = "Session expired or user not logged in." });
            }

            // Fetch all acknowledgment forms
            var result = await _post.GetAllClientPropertDeclarationFormAsync();

            if (result.Success && result.Data != null)
            {
                var allForms = result.Data as IEnumerable<ClientPropertyDeclaration>;

                if (allForms != null)
                {
                    // Filter forms based on the logged-in employee's name
                    var filteredForms = allForms
                        .Where(asset => asset.EmployeeName == loggedInEmployeeName)
                        .Select(asset => new
                        {
                            Id = asset.Id,
                            EmployeeName = asset.EmployeeName,
                            ClientName = asset.ClientName,
                        })
                        .ToList();

                    return Json(filteredForms);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                return Json(new { Success = false, Message = result.Message ?? "No acknowledgment forms found." });
            }
        }

        [HttpGet("PostJoining/GetClientDeclarationById/{EmpId}")]
        public async Task<JsonResult> GetClientDeclarationById([FromRoute] int EmpId)
        {
            try
            {
                var asset = await _post.GetClientPropertDeclarationByIdAsync(EmpId);

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

        [HttpGet("PostJoining/GetClientDeclaration/{ID}")]
        public async Task<JsonResult> GetClientDeclaration([FromRoute] int ID)
        {
            try
            {
                var asset = await _post.GetClientPropertDecByIdAsync(ID);

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

        public async Task<JsonResult> GetClientPropertyDeclaration()
        {
            // Fetch the result from the service layer
            var result = await _post.GetClientPropertDeclarationFormAsync();


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



                var Asset = result.Data as IEnumerable<ClientPropertyDeclaration>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        ClientName = asset.ClientName,

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


        #endregion



        #region Non Disclosure Agreement Form
        public IActionResult NDAForm()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        public IActionResult NDAFormDetails()
        {

            return View();
        }

        public async Task<IActionResult> SaveNDAForm(NDAFormViewModel model)
        {

            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;
            model.empId = Convert.ToInt32(empId);
            var result = await _post.CreateNDAFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";
                return RedirectToAction("NDAForm"); // Redirect back to the EmployeeType view

            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("NDAForm"); // Redirect back to the EmployeeType view
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetAllNDAForm()
        {
            // Retrieve JWT token from session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

            // Get the logged-in employee's name
            var loggedInEmployeeName = userDetails.userName;

            if (string.IsNullOrEmpty(loggedInEmployeeName))
            {
                return Json(new { Success = false, Message = "Session expired or user not logged in." });
            }

            // Fetch all acknowledgment forms
            var result = await _post.GetAllNDAFormAsync();

            if (result.Success && result.Data != null)
            {
                var allForms = result.Data as IEnumerable<NonDisclosureAgreement>;

                if (allForms != null)
                {
                    // Filter forms based on the logged-in employee's name
                    var filteredForms = allForms
                        .Where(asset => asset.EmployeeName == loggedInEmployeeName)
                        .Select(asset => new
                        {
                            Id = asset.Id,
                            EmployeeName = asset.EmployeeName,
                            AgreementDate = asset.AgreementDate,
                        })
                        .ToList();

                    return Json(filteredForms);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                return Json(new { Success = false, Message = result.Message ?? "No acknowledgment forms found." });
            }
        }

        [HttpGet("PostJoining/GetNDAFormById/{EmpId}")]
        public async Task<JsonResult> GetNDAFormById(int EmpId)
        {
            try
            {
                var asset = await _post.GetNDAFormByIdAsync(EmpId);

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

        public async Task<JsonResult> GetNDAForm()
        {
            // Fetch the result from the service layer
            var result = await _post.GetNDAFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<NonDisclosureAgreement>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        EmployeeName = asset.EmployeeName,
                        AgreementDate = asset.AgreementDate,

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

        //[HttpGet("PostJoining/GetNDAFormById/{ID}")]
        public async Task<JsonResult> GetNDAById([FromRoute] int ID)
        {
            try
            {
                var asset = await _post.GetNDAByIdAsync(ID);

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
