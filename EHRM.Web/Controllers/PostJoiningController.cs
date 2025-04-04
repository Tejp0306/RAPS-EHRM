using System.Security.Cryptography.Xml;
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PostJoiningForms()
        {
            return View();
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
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("PersonalInformationForm"); // Redirect back to the EmployeeType view
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
