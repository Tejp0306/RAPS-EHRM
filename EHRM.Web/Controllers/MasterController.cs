using EHRM.DAL.Database;
using EHRM.ServiceLayer.Master;
using EHRM.ViewModel.Master;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterService _master;
        private readonly string _fileStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        public MasterController(IMasterService master)
        {
             
            _master = master;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NoticeBoard()
        {
            return View();  
        }
        public IActionResult MsterRoles() { 
         return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveRoles(RoleViewModel model)
        {  // Check if the role exists based on the model ID
                if (model.Id > 0)
                {
                    // Update the role details
                    string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                    var updateResult = await UpdateRoleDetails(model.Id, updatedBy, model);

                    if (updateResult != null)
                    {
                        var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                        if (updateResponse?.success == true)
                        {
                            TempData["SuccessMessage"] = updateResponse?.message; // Store success message
                            return RedirectToAction("MsterRoles"); // Redirect to the list of roles
                        }
                        else
                        {
                            ViewBag.ErrorMessage = updateResponse?.message; // Display error message
                            return View(model); // Return to the same view with the provided model
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Unexpected error occurred during role update."; // Display generic error
                        return View(model); // Return to the same view with the provided model
                    }
                }
                else
                {
                    // Create a new role
                    string createdById = "waseem"; // Replace with logic to fetch the actual user ID
                    var result = await _master.CreateRoleAsync(model, createdById);

                    // Handle the result of the create operation
                    if (result.Success)
                    {
                        TempData["SuccessMessage"] = result.Message; // Store success message
                        return RedirectToAction("MsterRoles"); // Redirect to the list of roles
                    }
                    else
                    {
                        ViewBag.ErrorMessage = result.Message; // Display error message
                        return View(model); // Return to the same view with the provided model
                    }
                }
           
        }

        [HttpGet]
        public async Task<JsonResult> GetAllRolesData()
        {
            // Fetch the result from the service layer
            var result = await _master.GetAllRolesAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var roles = result.Data as IEnumerable<Role>;
                if (roles != null)
                {
                    // Return the list of roles as a JSON response
                    var roleList = roles.Select(role => new
                    {
                        Id = role.Id,
                        RoleName = role.RoleName,
                        RoleDescription = role.RoleDescription
                    }).ToList();
                    return Json(roleList);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No roles found." });
            }
        }



        [NonAction]
        private async Task<object> UpdateRoleDetails(int id, string updatedBy, RoleViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _master.UpdateRoleAsync(id, updatedBy, model);

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
                    message = "An error occurred while updating the role. Please try again later."
                };
            }
        }

        [HttpGet("Master/GetRoleDetails/{roleID}")]
        public async Task<JsonResult> GetRoleDetails([FromRoute] int roleID)
        {
            try
            {
                var role = await _master.GetRoleByIdAsync(roleID);

                if (role == null)
                {
                    return Json(new { success = false, message = "Role not found." });
                }

                return Json(new { success = true, data = role });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the notice details." });
            }
        }

    }
}
    #endregion


