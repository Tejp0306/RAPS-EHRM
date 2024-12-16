using EHRM.ServiceLayer.MainMenuRepo;
using EHRM.ViewModel.MainMenu;
using EHRM.ViewModel.Master;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMainMenuService _mainmaster;

        public MenuController(IMainMenuService mainmaster)
        {

            _mainmaster = mainmaster;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MainMenuPage()
        {
            return View();

        }
        //Save Main Menu Controller
        public async Task<IActionResult> SaveMainMenu(MainMenuViewModel model)
        {  // Check if the role exists based on the model ID

            //string createdById = "waseem"; // Replace with logic to fetch the actual user ID

            if (model.Id > 0)
            {
                // Update the role details
                string updatedBy = "Arjun"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdatemainmenuDetails(model.Id, updatedBy, model);

                if (updateResult != null)
                {

                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record Has been updated ";
                        return RedirectToAction("MainMenuPage"); // Redirect to the list of roles
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



            var result = await _mainmaster.CreateMainMenuAsync(model);


            // Handle the result of the create operation
            if (result.Success)
            {

                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Operation completed successfully!";
                return RedirectToAction("MainMenuPage"); // Redirect to the list of roles
            }
            else
            {
                ViewBag.ErrorMessage = result.Message; // Display error message
                return View(model); // Return to the same view with the provided model
            }
            

        }
        [HttpGet]
        public async Task<JsonResult> GetAllMenuData()

        {
            // Fetch the result from the service layer
            var result = await _mainmaster.GetAllMainMenuAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                // The data is already a list of anonymous types with TeamName included
                var MainMenu = result.Data as IEnumerable<dynamic>;
                if (MainMenu != null)
                {
                    // Use Select to map the holidays to the desired output format
                    var MenuList = MainMenu.Select(menu => new
                    {
                        Id = menu.Id,
                        Name = menu.Name,
                        Icon = menu.Icon,


                    }).ToList(); // Convert to a List
                    
                    
                    return Json(MenuList);

                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No holidays found." });
            }
        }

        public async Task<IActionResult> DeleteMainMenu(int id)
        {
            try
            {
                // Call the service method to delete the notice from the database
                var result = await _mainmaster.DeleteMainMenuAsync(id);

                // Check the result and provide feedback to the user
                if (result.Success)
                {
                    return Json(new { Success = true, Message = "Notice deleted successfully!" });
                }
                else
                {
                    return Json(new { Success = true, Message = "Notice not deleted !" });
                }
            }
            catch (Exception ex)
            {
                // Store the error message in TempData if an exception occurs
                TempData["ErrorMessage"] = $"Error deleting the notice: {ex.Message}";
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        private async Task<object> UpdatemainmenuDetails(int id, string updatedBy, MainMenuViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _mainmaster.UpdateMainMenuAsync(id, updatedBy, model);

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



        // Get Holiday based on Id for the edit button

        [HttpGet("Menu/GetMainMenuDetails/{mainmenuID}")]
        public async Task<JsonResult> GetMainMenuDetails([FromRoute] int mainmenuID)
        {
            try
            {
                var mainmenu = await _mainmaster.GetmainmenuByIdAsync(mainmenuID);

                if (mainmenu == null)
                {
                    return Json(new { success = false, message = "holiday not found." });
                }

                return Json(new { success = true, data = mainmenu });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the holiday details." });
            }
        }

    }
}
