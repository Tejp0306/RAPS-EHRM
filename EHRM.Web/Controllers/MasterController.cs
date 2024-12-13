using EHRM.DAL.Database;
using EHRM.ServiceLayer.Master;
using EHRM.ViewModel.Master;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterService _master;
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
        public IActionResult MsterRoles()
        {
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
                return Json(new { success = false, message = "An error occurred while retrieving the role details." });
            }
        }


        #region Starting Holidays
        [HttpGet]

        public IActionResult AddHoliday()
        {
            return View();

        }

        [NonAction]
        private async Task<object> UpdateHolidayDetails(int id, string updatedBy, HolidayViewModel model)
        {
            try
            {
                // Call the service method to update the role
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
                    message = "An error occurred while updating the role. Please try again later."
                };
            }
        }



        // Get Holiday based on Id for the edit button

        [HttpGet("Master/GetHolidayDetails/{holidayId}")]
        public async Task<JsonResult> GetHolidayDetails([FromRoute] int holidayID)
        {
            try
            {
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
                // Call the service method to delete the notice from the database
                var result = await _master.DeleteHolidayAsync(id);

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

        //Post Handle Addholiday form submission
        [HttpPost]
        public async Task<IActionResult> SaveHoliday(HolidayViewModel model)
        {  // Check if the role exists based on the model ID
            if (model.Id > 0)
            {

                string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateHolidayDetails(model.Id, updatedBy, model);

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
                // Create a new holiday
                string createdById = "waseem"; // Replace with logic to fetch the actual user ID
                var result = await _master.CreateHolidayAsync(model, createdById);

                // Handle the result of the create operation
                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message; // Store success message
                    return RedirectToAction("AddHoliday"); // Redirect to the list of roles
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message; // Display error message
                    return View(model); // Return to the same view with the provided model
                }
            }

        }

        //public async Task<IActionResult> AddHoliday(HolidayViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // If validation fails, return the same view with the model and error messages
        //        return View(model);
        //    }

        //    // Get the currently logged-in user (replace with your actual logic to retrieve the user)
        //    string createdBy = "Arjun";

        //    // Call the service to create a new holiday
        //    var result = await _master.CreateHolidayAsync(model, createdBy);

        //    if (result.Success)
        //    {
        //        // If successful, store a success message in TempData and redirect to a list view or confirmation page
        //        TempData["SuccessMessage"] = result.Message;
        //        return RedirectToAction("AddHoliday");
        //    }
        //    else
        //    {
        //        // If there's an error, add it to the ModelState and return the view with the error
        //        ModelState.AddModelError(string.Empty, result.Message);
        //        return View(model);
        //    }
        //}

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
                        id = team.Id, // Ensure Team class has an Id property
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

        #region Emp_Type
        public IActionResult EmployeeType()
        {
            return View();
        }

        [NonAction]
        private async Task<object> UpdateEmployeeTypeDetails(int id,  EmployeeTypeViewModel model)
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
            catch (Exception ex)
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
            if (model.Id > 0 )
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
                        TempData["ToastMessage"] = "Record Has been updated ";
                        return RedirectToAction("EmployeeType"); // Redirect to the list of roles
                    }
                    else
                    {
                        TempData["ToastType"] = "error"; // Store error message
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
                    TempData["ToastType"] = "error"; // Store error message
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
            catch (Exception ex)
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
