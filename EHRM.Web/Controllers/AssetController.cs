using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Asset;
using EHRM.ServiceLayer.MainMenuRepo;
using EHRM.ServiceLayer.Master;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Master;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{

    public class AssetController : Controller

    {
        private readonly IAssetService _Asset;
        private readonly IUnitOfWork _unitOfWork;

        public AssetController(IAssetService Asset, IUnitOfWork unitOfWork)
        {

            _Asset = Asset;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddAsset()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllTeamData()
        {
            // Fetch the result from the service layer
            var result = await _Asset.GetTeamAsync();
            TeamViewModel teamViewModel = new TeamViewModel();
            if (result.Success && result.Data != null)
            {
                // Attempt to cast result.Data to IEnumerable<Team>
                if (result.Data is IEnumerable<Team> teams)
                {
                    // Project the team list to a simplified JSON-friendly format
                    var teamList = teams.Select(team => new
                    {
                        id = team.TeamId, // Ensure Team class has an Id property
                        name = team.Name // Ensure Team class has a Name property
                    }).ToList();

                    return Json(new { Success = true, Data = teamList });
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format (IEnumerable<Team>)." });
                }
            }
            else
            {
                // Handle failure scenarios
                return Json(new { Success = false, Message = result.Message ?? "No teams found." });
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetEmployeebyTeamId(int TeamId)
        {
            try
            {
                var ts = await _Asset.GetEmployeeByTeamIdAsync(TeamId);

                if (ts == null)
                {
                    return Json(new { success = false, message = "Employee not found." });
                }

                return Json(new { success = true, data = ts });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Team details." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            try
            {
                // Call the service method to delete the notice from the database
                var result = await _Asset.DeleteAssetAsync(id);

                // Check the result and provide feedback to the user
                if (result.Success)
                {
                    return Json(new { Success = true, Message = "Asset deleted successfully!" });
                }
                else
                {
                    return Json(new { Success = true, Message = "Asset not deleted !" });
                }


            }
            catch (Exception ex)
            {
                // Store the error message in TempData if an exception occurs
                TempData["ErrorMessage"] = $"Error deleting the Asset: {ex.Message}";
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsset(AssetViewModel model)
        {  // Check if the role exists based on the model ID
            if (model.Id > 0)
            {
                // Update the Asset details
                int updatedBy = 1; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateAssetDetails(model.Id, updatedBy, model);
                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Asset Has been updated ";
                        return RedirectToAction("AddAsset"); // Redirect to the list of roles
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse?.message; // Display error message
                        return View(model); // Return to the same view with the provided model
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Unexpected error occurred during Asset update."; // Display generic error
                    return View(model); // Return to the same view with the provided model
                }
            }
            else
            {
                // Create a new Asset
                int createdById = 1; // Replace with logic to fetch the actual user ID
                var result = await _Asset.CreateAssetAsync(model, createdById);
                // Handle the result of the create operation
                if (result.Success)
                {
                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Asset created successfully!";
                    return RedirectToAction("AddAsset"); // Redirect to the list of roles
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message; // Display error message
                    return View(model); // Return to the same view with the provided model
                }
            }
        }

        [NonAction]
        private async Task<object> UpdateAssetDetails(int id, int updatedBy, AssetViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _Asset.UpdateAssetAsync(id, updatedBy, model);

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

        [HttpGet]
        public async Task<JsonResult> GetAllAssetData()
        {
            // Fetch the result from the service layer
            var result = await _Asset.GetAllAssetAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Asset = result.Data as IEnumerable<dynamic>;
                if (Asset != null)
                {
                    var AssetList = Asset.Select(asset => new
                    {
                        Id = asset.Id,
                        Name = asset.Name,

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

        [HttpGet("Asset/GetAssetDetails/{assetID}")]
        public async Task<JsonResult> GetAssetDetails([FromRoute] int assetID)
        {
            try
            {
                var asset = await _Asset.GetAssetByIdAsync(assetID);

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
    }
}