using EHRM.DAL.Database;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.PostJoining;
using EHRM.ServiceLayer.SuperAdmin;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.PostJoining;
using EHRM.ViewModel.SuperAdmin;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminService _superAdmin;

        public SuperAdminController(ISuperAdminService superAdmin)
        {

            _superAdmin = superAdmin;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region Tenant Registration

        public IActionResult TenantRegistration()
        {
            return View();
        }

        public IActionResult TenantFormDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveTenantRegistrationForm(TenantRegistrationViewModel model)
        {
            // Check if the model contains a valid ID (indicating an update operation)
            if (model.Id > 0)
            {
                // Update the tenant registration form
                var result = await UpdateTenantDetails(model.Id, model);

                if (result != null)
                {
                    var updateResponse = result as dynamic; // Assuming it's returning an anonymous type

                    if (updateResponse?.success == true)
                    {
                        // Success: Update message and redirect
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Tenant registration details have been updated successfully!";
                        return RedirectToAction("TenantRegistration"); // Redirect back to the tenant registration view
                    }
                    else
                    {
                        // Error: Update error message and return to the view
                        TempData["ToastType"] = "danger"; // Store error message
                        TempData["ToastMessage"] = "An error occurred while updating the tenant registration form.";
                        return RedirectToAction("TenantRegistration"); // Return to the tenant registration view with the provided model
                    }
                }
                else
                {
                    // Unexpected error: Display generic error message
                    TempData["ToastType"] = "danger"; // Store error type
                    TempData["ToastMessage"] = "Unexpected error occurred during tenant registration update."; // Display generic error
                    return RedirectToAction("TenantRegistration"); // Return to the same view
                }
            }
            else
            {
                // Create a new tenant registration form
                var result = await _superAdmin.CreateTenantRegistrationFormAsync(model);

                // Handle the result of the create operation
                if (result.Success)
                {
                    // Success: Create success message and redirect
                    TempData["ToastType"] = "success"; // Store success message
                    TempData["ToastMessage"] = "Tenant registration form has been submitted successfully!";
                    return RedirectToAction("TenantRegistration"); // Redirect to the tenant registration view
                }
                else
                {
                    // Error: Create error message and return to the view
                    TempData["ToastType"] = "danger"; // Store error message
                    TempData["ToastMessage"] = "An error occurred while submitting the tenant registration form.";
                    return RedirectToAction("TenantRegistration"); // Return to the same view with the provided model
                }
            }
        }

        [NonAction]
        private async Task<object> UpdateTenantDetails(int id, TenantRegistrationViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _superAdmin.UpdateTenantFormAsync(id, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception ex)
            {
                // Log the exception (for debugging or error tracking)
                // _logger.LogError(ex, "Error occurred while updating role details for role ID: {RoleId}", id);

                // Return a generic error response
                return new
                {
                    success = false,
                    message = "An error occurred while updating the Team. Please try again later."
                };
            }
        }

        //[HttpGet("SuperAdmin/GetTenantRegistrationDetails/{ID}")]
        public async Task<JsonResult> GetTenantRegistrationDetails(int ID)
        {
            try
            {
                var nb = await _superAdmin.GetTenantRegistrationFormAsync(ID);

                if (nb == null)
                {
                    return Json(new { success = false, message = "Tenant Registration not found." });
                }

                //RedirectToAction("TenantRegistration", "SuperAdmin", new { id = 123 });

                return Json(new { success = true, data = nb });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Tenant Registrations." });
            }

        }




        //public async Task<JsonResult> GetTenantRegistrationDetails(int ID)
        //{
        //    try
        //    {
        //        var nb = await _superAdmin.GetTenantRegistrationFormAsync(ID);

        //        if (nb == null)
        //        {
        //            return Json(new { success = false, message = "Tenant Registration not found." });
        //        }


        //        return Json(new { success = true, data = nb });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "An error occurred while retrieving the Tenant Registrations." });
        //    }

        //}

        [HttpGet]
        public async Task<JsonResult> GetTenantForm()
        {
            // Fetch the result from the service layer
            var result = await _superAdmin.GetTenantFormAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Tenant = result.Data as IEnumerable<Client>;
                if (Tenant != null)
                {
                    var TenantList = Tenant.Select(asset => new
                    {
                        Id = asset.Id,
                        OrganizationId = asset.OrganizationId,
                        OrganizationName = asset.OrganizationName,
                        AdminName = asset.AdminName,

                    }).ToList();

                    return Json(TenantList);
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

        [HttpGet("SuperAdmin/GetTennat/{tenantID}")]
        public async Task<JsonResult> GetTennat([FromRoute] int tenantID)
        {
            try
            {
                var asset = await _superAdmin.GetTenantDetailsAsync(tenantID);

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
