using System.Security.Claims;
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

        public IActionResult AddNoticeBoard()
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
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record Has been updated ";
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
                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Operation completed successfully!";
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

        //public IActionResult AddNoticeBoard()
        //{
        //    return View();  
        //}

        [HttpPost]
        [ValidateAntiForgeryToken] // Security measure to protect against CSRF attacks
        public async Task<IActionResult> CreateNotice(AddNoticeBoardViewModel model)
        {
            if (model.Id == 0)
            {
                try
                {
                    // Get the current user ID (if using session or logged-in user info)
                    int createdBy = 1; // Replace with the actual logic to fetch the current user's ID
                    // For example: var createdBy = _userService.GetCurrentUserId();
                    var filepath = "";
                    if (model.File != null)
                    {
                        filepath = Upload(model); // Assume Upload is a method that handles file uploads
                    }
                    // Call the service method to create the notice in the database
                    var result = await _master.CreateAddNoticeBoardAsync(model, createdBy, filepath);

                    // Check if the result indicates a successful creation
                    if (result.Success)
                    {
                        // Success: Store success message and toast
                        TempData["ToastType"] = "success"; // Success toast type
                        TempData["ToastMessage"] = "Record has been created"; // Success message
                        return RedirectToAction("AddNoticeBoard"); // Redirect to the list of notices
                    }
                    else
                    {
                        // Error: Store error message and toast
                        TempData["ToastType"] = "danger"; // Error toast type
                        TempData["ToastMessage"] = "An error occurred while creating the record."; // Error message
                        return RedirectToAction("AddNoticeBoard"); // Return to the same page
                    }

                }
                catch (Exception ex)
                {
                    // Handle unexpected errors
                    TempData["ToastType"] = "danger"; // Error toast type
                    TempData["ToastMessage"] = $"Error creating notice: {ex.Message}"; // Error message
                    return RedirectToAction("AddNoticeBoard"); // Return to the same page
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
                        // Success: Store success message and toast
                        TempData["ToastType"] = "success"; // Success toast type
                        TempData["ToastMessage"] = "Record has been updated"; // Success message
                        return RedirectToAction("AddNoticeBoard"); // Redirect to the list of notices
                    }
                    else
                    {
                        // Error: Store error message and toast
                        TempData["ToastType"] = "danger"; // Error toast type
                        TempData["ToastMessage"] = "An error occurred while updating the record."; // Error message
                        return View(model); // Return to the same view with the provided model
                    }
                }
                else
                {
                    // Unexpected error: Display generic error message
                    TempData["ToastType"] = "danger"; // Error toast type
                    TempData["ToastMessage"] = "Unexpected error occurred during notice update."; // Error message
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

        #region Starting Holidays

        [HttpGet]
        //Action method to Show the view of holiday screen
        public IActionResult AddHoliday()
        {
            return View();

        }

        [NonAction]
        //Update Holiday Details
        private async Task<object> UpdateHolidayDetails(int id, string updatedBy, HolidayViewModel model)
        {
            try
            {
                // Call the service method to update the Holiday
                var result = await _master.UpdateHolidayAsync(id, updatedBy, model);

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
                    message = "An error occurred while updating the Holiday. Please try again later."
                };
            }
        }
        
        // Get Holiday based on Id for the edit button functionality

        [HttpGet("Master/GetHolidayDetails/{holidayId}")]
        public async Task<JsonResult> GetHolidayDetails([FromRoute] int holidayID)
        {
            try
            {
                // Call the service method to Get holiday based on the ID
                var holiday = await _master.GetHolidayByIdAsync(holidayID);

                if (holiday == null)
                {
                    return Json(new { success = false, message = "holiday not found." });
                }

                return Json(new { success = true, data = holiday });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the holiday details." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            try
            {
                // Call the service method to delete the Holiday from the database
                var result = await _master.DeleteHolidayAsync(id);

                // Toaster message if Deleted successfully
                if (result.Success)
                {
                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Holiday Deleted Successfully"; // Store success message
                    return Json(new { Success = true, Message = "Holiday deleted !" });
                }
                else
                {
                    return Json(new { Success = true, Message = "Holiday not deleted !" });
                }


            }
            catch (Exception ex)
            {
                // Store the error message in TempData if an exception occurs
                TempData["ErrorMessage"] = $"Error deleting the Holiday: {ex.Message}";
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        //Post Handle Addholiday form submission
        [HttpPost]
        public async Task<IActionResult> SaveHoliday(HolidayViewModel model)
        {  // Check if the holiday data ID exists based on the model ID
            if (model.Id > 0)
            {

                string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateHolidayDetails(model.Id, updatedBy, model);

                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success";
                        TempData["ToastMessage"] = "Holiday Updated Successfully"; // Store success message
                        return RedirectToAction("addholiday"); // Redirect to the list of Holiday
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
                // Create a new holiday
                string createdById = "waseem"; // Replace with logic to fetch the actual user ID
                var result = await _master.CreateHolidayAsync(model, createdById);

                // Handle the result of the create operation
                if (result.Success)
                {
                    TempData["ToastType"] = "success"; // Store success message
                    TempData["ToastMessage"] = "Holiday Saved Successfully";
                    return RedirectToAction("AddHoliday"); // Redirect to the list of Holiday
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message; // Display error message
                    return View(model); // Return to the same view with the provided model
                }
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAllTeamData()
        {
            // Fetch the result from the service layer
            var result = await _master.GetTeamAsync();

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

        //Get All Holiday Data

        [HttpGet]
        public async Task<JsonResult> GetAllHolidayData()

        {
            // Fetch the result from the service layer
            var result = await _master.GetAllHolidayAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                // The data is already a list of anonymous types with TeamName included
                var holidays = result.Data as IEnumerable<dynamic>;
                if (holidays != null)
                {
                    // Use Select to map the holidays to the desired output format
                    var holidayList = holidays.Select(holiday => new
                    {
                        Id = holiday.Id,
                        Name = holiday.Name,
                        Description = holiday.Description,
                        HolidayDate = holiday.HolidayDate

                    }).ToList(); // Convert to a List

                    return Json(holidayList);
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
        #endregion
        
        #region Team Screen
        public IActionResult TeamScreen()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTeamScreen(int id)
        {
            try
            {
                // Call the service method to delete the notice from the database
                var result = await _master.DeleteTeamAsync(id);

                // Check the result and provide feedback to the user
                if (result.Success)
                {
                    return Json(new { Success = true, Message = "Team deleted successfully!" });
                }
                else
                {
                    return Json(new { Success = true, Message = "Team not deleted !" });
                }


            }
            catch (Exception ex)
            {
                // Store the error message in TempData if an exception occurs
                TempData["ErrorMessage"] = $"Error deleting the Team: {ex.Message}";
                return Json(new { Success = false, Message = ex.Message });
            }
        }
        [NonAction]
        private async Task<object> UpdateTeamDetails(int id, int updatedBy, TeamScreenViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _master.UpdateTeamAsync(id, updatedBy, model);

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
        [HttpPost]
        public async Task<IActionResult> SaveTeam(TeamScreenViewModel model)
        {
            // Check if the role exists based on the model ID
            if (model.Id > 0)
            {
                
                // Update the role details
                int updatedBy = 1; // Replace with actual logic to fetch the current user ID
                var result = await UpdateTeamDetails(model.Id, updatedBy, model);

                if (result != null)
                {
                    var updateResponse = result as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        // Success: Update message and redirect

                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record has been updated ";
                        return RedirectToAction("TeamScreen"); // Redirect to the list of role
                    }
                    else
                    {
                        // Error: Update error message and return to the view
                        TempData["ToastType"] = "danger"; // Store error message
                        TempData["ToastMessage"] = "An error occurred while updating the record."; ; // Store error message
                        return RedirectToAction("TeamScreen"); // Return to the same view with the provided model
                    }
                }
                else
                {
                    // Unexpected error: Display generic error message
                    TempData["ToastType"] = "danger"; // Store error type
                    TempData["ToastMessage"] = "Unexpected error occurred during role update."; // Display generic error
                    return RedirectToAction("TeamScreen"); // Return to the same view
                }
            }
            else
            {
                // Create a new team
                int createdById = 1; // Replace with logic to fetch the actual user ID
                var result = await _master.CreateTeamAsync(model, createdById);

                // Handle the result of the create operation
                if (result.Success)
                {
                    // Success: Create success message and redirect

                    TempData["ToastType"] = "success"; // Store success message
                    TempData["ToastMessage"] = "Record has been created ";
                    return RedirectToAction("TeamScreen"); // Redirect to the list of teams
                }
                else
                {
                    // Error: Create error message and return to the view
                    TempData["ToastType"] = "danger"; // Store error message
                    TempData["ToastMessage"] = "An error occurred while creating the record."; ; /// Store error message
                    return RedirectToAction("TeamScreen"); // Return to the same view with the provided model
                }
                }
            }

       
        [HttpGet]
        public async Task<JsonResult> GetAllTeamScreenData()
        {
            // Fetch the result from the service layer
            var result = await _master.GetAllTeamAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var team = result.Data as IEnumerable<Team>;
                if (team != null)
                {
                    // Return the list of Team as a JSON response
                    var TeamScreenList = team.Select(team => new
                    {
                        Id = team.Id,
                        Name = team.Name,
                        Description = team.Description
                    }).ToList();
                    return Json(TeamScreenList);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No Team found." });
            }
        }
        [HttpGet("Master/GetTeamScreenDetails/{TeamScreenID}")]
        public async Task<JsonResult> GetTeamScreenDetails([FromRoute] int TeamScreenID)
        {
            try
            {
                var ts = await _master.GetTeamByIdAsync(TeamScreenID);

                if (ts == null)
                {
                    return Json(new { success = false, message = "Team not found." });
                }

                return Json(new { success = true, data = ts });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Team details." });
            }
        }

        #endregion


        #region Emp_Type
        public IActionResult EmployeeType()
        {
            return View();
        }

        [NonAction]
        private async Task<object> UpdateEmployeeTypeDetails(int id, EmployeeTypeViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _master.UpdateEmployeeTypeAsync(id, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception)
            {
                // Log the exception (for debugging or error tracking)
                // _logger.LogError(ex, "Error occurred while updating role details for role ID: {RoleId}", id);

                // Return a generic error response
                return new
                {
                    success = false,
                    message = "An error occurred while updating the Employee Type. Please try again later."
                };
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveEmployeeType(EmployeeTypeViewModel model)
        {  // Check if the exists based on the model ID
            if (model.Id> 0)
            {
                // Update the role details
                //string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateEmployeeTypeDetails(model.Id, model);

                if (updateResult != null)
                {

                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record has been updated ";
                        return RedirectToAction("EmployeeType"); // Redirect to the list of roles
                    }
                    else
                    {
                        TempData["ToastType"] = "danger"; // Store error message
                        TempData["ToastMessage"] = "An error occurred while updating the record.";
                        return RedirectToAction("EmployeeType");  // Return to the same view with the provided model
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Unexpected error occurred during Employee Type update."; // Display generic error
                    return View(model); // Return to the same view with the provided model
                }
            }
            else
            {
                // Create a new role
                // string createdById = "waseem"; // Replace with logic to fetch the actual user ID
                var result = await _master.CreateEmployeeTypeAsync(model);

                // Handle the result of the create operation
                if (result.Success)
                {
                    // Error handling for the case where creation fails
                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Record saved successfully!";
                    return RedirectToAction("EmployeeType"); // Redirect to the list of roles
                }
                else
                {
                    // Error handling for the case where creation fails
                    TempData["ToastType"] = "danger"; // Store error message
                    TempData["ToastMessage"] = "An error occurred while creating the record.";
                    return RedirectToAction("EmployeeType"); // Redirect back to the EmployeeType view
                }
            }
        }

        [HttpGet("Master/GetEmployeeTypeDetails/{ID}")]
        public async Task<JsonResult> GetEmployeeTypeDetails([FromRoute] int ID)
        {
            try
            {
                var et = await _master.GetEmployeeTypeIdAsync(ID);

                if (et == null)
                {
                    return Json(new { success = false, message = "Employee Type not found." });
                }

                return Json(new { success = true, data = et });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the employee Type details." });
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetAllEmployeeTypeData()
        {
            // Fetch the result from the service layer
            var result = await _master.GetAllEmployeeTypeAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var emptype = result.Data as IEnumerable<EmpType>;
                if (emptype != null)
                {
                    // Return the list of roles as a JSON response
                    var employeeTypeList = emptype.Select(emptype => new
                    {
                        Id = emptype.Id,
                        EmpType1 = emptype.EmpType1,
                        //RoleDescription = role.RoleDescription
                    }).ToList();
                    return Json(employeeTypeList);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No Employee Type found." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEmployeeType(int id)
        {
            try
            {
                // Call the service method to delete the notice from the database
                var result = await _master.DeleteEmployeeTypeAsync(id);

                // Check the result and provide feedback to the user
                if (result.Success)
                {
                    return Json(new { Success = true, Message = "Employee Type deleted successfully!" });
                }
                else
                {
                    return Json(new { Success = true, Message = "Employee Type not deleted !" });
                }


            }
            catch (Exception ex)
            {
                // Store the error message in TempData if an exception occurs
                TempData["ErrorMessage"] = $"Error deleting the Employee Type: {ex.Message}";
                return Json(new { Success = false, Message = ex.Message });
            }
        }


        #endregion
    }
}