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
                return Json(new { success = false, message = "An error occurred while retrieving the role details." });
            }
        }


        #region  NOTICE FORM

        public IActionResult AddNoticeBoard()
        {
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Security measure to protect against CSRF attacks
        public async Task<IActionResult> CreateNotice(AddNoticeBoardViewModel model)
        {
            if (model.Id == 0)
            {
                try
                {
                    // Get the current user ID (if using session or logged-in user info)
                    int createdBy = 1; // Replace this with the actual logic for fetching current user's ID
                    // For example: var createdBy = _userService.GetCurrentUserId();
                    var filepath = Upload(model);
                    // Call the service method to create the notice in the database
                    var result = await _master.CreateAddNoticeBoardAsync(model, createdBy, filepath);

                    // Check if the result indicates a successful creation
                    if (result.Success)
                    {
                        TempData["SuccessMessage"] = result.Message; // Store success message to show after redirection
                        return RedirectToAction("AddNoticeBoard"); // Redirect to a list page or a page displaying notices
                    }
                    else
                    {
                        // If creation failed, add error message to TempData or ViewBag
                        TempData["ErrorMessage"] = result.Message;
                    }

                }
                catch (Exception ex)
                {
                    // Handle unexpected errors
                    TempData["ErrorMessage"] = $"Error creating notice: {ex.Message}";
                }
                return View(model);
            }
            else
            {

                var updateResult = await UpdateAddNoticeBoard(model.Id, model);

                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["SuccessMessage"] = updateResponse?.message; // Store success message
                        return RedirectToAction("AddNoticeBoard"); // Redirect to the list of roles
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse?.message; // Display error message
                        return View(model); // Return to the same view with the provided model
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Unexpected error occurred during Notice update."; // Display generic error
                    return View(model); // Return to the same view with the provided model
                }
            }
        }
        private string Upload(AddNoticeBoardViewModel model)
        {
            // Check if a file is provided, if not, simply return null (indicating no file upload)
            if (model.File == null || model.File.Length == 0)
            {
                return null; // No file uploaded, return null or an empty string
            }

            // Define the directory path to store files
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
          

            // Create the folder if it doesn't exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Generate a unique file name to avoid name conflicts (optional, can use the original name)
            FileInfo fileInfo = new FileInfo(model.File.FileName);
            string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;  // Unique file name generation
                                                                               // You can also use model.FileName here if you want to allow users to specify the name

            // Combine path with the file name to get the full file path
           string fileNameWithPath = Path.Combine(path, fileName);

            // Save the file to the specified directory
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            // Return the full file path or file name
            return Path.Combine("\\Files",fileName);
        }


        [NonAction]
        private async Task<object> UpdateAddNoticeBoard(int id,  AddNoticeBoardViewModel model)
        {
            try
            {
                int updatedBy = 1;
                // Call the service method to update the role
                var result = await _master.UpdateAddNoticeBoardAsync(id, updatedBy, model); 

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = "Notice Updated"
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


        [HttpPost]
        public async Task<IActionResult> DeleteAddNoticeBoard(int id)
        {
            try
            {
                // Call the service method to delete the notice from the database
                var result = await _master.DeleteAddNoticeBoardAsync(id);

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
                return Json(new { Success = false , Message = ex.Message});
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAllAddNoticeBoard()
        {
            // Fetch the result from the service layer
            var result = await _master.GetAllAddNoticeBoardAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var noticeBoards = result.Data as IEnumerable<NoticeBoard>;
                if (noticeBoards != null)
                {
                    // Return the list of NoticeBoards as a JSON response
                    var noticeBoardList = noticeBoards.Select(noticeBoard => new
                    {
                        Id = noticeBoard.Id,
                        HeadingName = noticeBoard.HeadingName,
                        Description = noticeBoard.Description
                    }).ToList();

                    return Json(noticeBoardList);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No NoticeBoards found." });
            }
        }
      

        [HttpGet]
        public async Task<JsonResult> GetNoticeBoardDetails(int NoticeBoardID)
        {
            try
            {
                var nb = await _master.GetAllAddNoticeBoardByIdAsync(NoticeBoardID);

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

        public async Task<IActionResult> ShowFile(int id)
        {
            try
            {
                var nb = await _master.GetAllAddNoticeBoardByIdAsync(id);

                if (nb == null)
                {
                    return Json(new { success = false, message = "Notice not found." });
                }


                return Json(new { success = true, data = nb });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the notice details." });
            }
        }

       
    }



}
    #endregion


