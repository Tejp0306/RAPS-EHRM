using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Review;
using EHRM.ViewModel.Review;

using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _review;
        private readonly IUnitOfWork _UnitOfWork;

        public ReviewController(IReviewService review, IUnitOfWork unitOfWork)
        {

            _review = review;
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProbationDashboard()
        {
            return View();
        }

        public async Task<IActionResult> ProbationEvaluation()
        {
            try
            {
                // Fetch the probation data
                var result = await _review.GetAllProbationDataAsync();

                if (result.Success && result.Data != null)
                {
                    // Attempt to cast result.Data to IEnumerable<Team>
                    if (result.Data is IEnumerable<ProbationEvaluationQuestion> ques)
                    {
                        // Project the team list to a simplified JSON-friendly format
                        var questions = ques.Select(q => new EvaluationQuestion
                        {
                            Id = q.QuestionId,
                            Question = q.Question
                        }).ToList();

                        return View(questions);
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
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during data retrieval or mapping
                // You could log this or display an error message
                Console.WriteLine($"Error fetching probation data: {ex.Message}");
                return View("Error"); // Show an error view or a user-friendly message
            }
        }




        public IActionResult AddReview()
        {
            return View();
        }

        //Get Probation Question Data from Db

        [HttpGet]
        public async Task<JsonResult> GetAllProbationData()
        {
            // Fetch the result from the service layer
            var result = await _review.GetAllProbationDataAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var Probation = result.Data as IEnumerable<ProbationEvaluationQuestion>;
                if (Probation != null)
                
                {
                    // Return the list of Team as a JSON response
                    var ProbationQuestionList = Probation.Select(probation => new
                    {
                        id = probation.QuestionId,
                        Question =probation.Question

                    }).ToList();
                    return Json(ProbationQuestionList);
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

        private async Task<object> UpdateQuestionDetails(int id, EvaluationQuestion model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _review.UpdateQuestionDetailsAsync(id,model);

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
                    message = "An error occurred while updating the Question. Please try again later."
                };
            }
        }

        //Save Questions to the DataBase
        [HttpPost]
        public async Task<IActionResult> SaveQuestions(EvaluationQuestion model)
        {  // Check if the Question exists based on the model ID
            if (model.Id > 0)
            {
                //Update the role details
                
                var updateResult = await UpdateQuestionDetails(model.Id, model);
                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record Has been updated ";
                        return RedirectToAction("AddReview"); // Redirect to the list of roles
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
                string createdById = "Arjun"; // Replace with logic to fetch the actual user ID
                var result = await _review.CreateQuestionAsync(model, createdById);
                // Handle the result of the create operation
                if (result.Success)
                {
                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Operation completed successfully!";
                    return RedirectToAction("AddReview"); // Redirect to the list of roles
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message; // Display error message
                    return View(model); // Return to the same view with the provided model
                }
            }
        }


        [HttpGet("Review/GetQuestionData/{questionID}")]
        public async Task<JsonResult> GetQuestionData([FromRoute] int questionID)
        {
            try
            {
                var ques = await _review.GetQuestionByIdAsync(questionID);

                if (ques == null)
                {
                    return Json(new { success = false, message = "Role not found." });
                }

                return Json(new { success = true, data = ques });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the notice details." });
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                // Call the service method to delete the Holiday from the database
                var result = await _review.DeleteReviewAsync(id);

                // Toaster message if Deleted successfully
                if (result.Success)
                {
                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Holiday Deleted Successfully"; // Store success message
                    return RedirectToAction("addholiday");
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

        public IActionResult DeclarationDashboard()
        {
            return View();
        }

        // Non-action method to check profile completion status
        [NonAction]
        private string CheckProfileStatus(bool isProfileCompleted)
        {
            if (!isProfileCompleted)
            {
                return "Profile Incomplete";
            }

            return "Profile Complete";
        }

        public async Task<JsonResult> GetAllDetailsData()
        {
            // Fetch the result from the service layer
            var result = await _review.GetAllDetailsAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var detail = result.Data as IEnumerable<EmployeesDeclaration>;
                if (detail != null)
                {
                    // Return the list of roles as a JSON response
                    var detailList = detail.Select(detail => new
                    {
                        Id = detail.EmpId,
                        Name = detail.EmployeeName,
                        Email = detail.OfficialEmail,
                        ProfileStatus = CheckProfileStatus(detail.IsActive)
                    }).ToList();
                    return Json(detailList);
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
        [HttpPost]
        public async Task<JsonResult> ActivateAccount(int empId)
        {
            try
            {
                // Call the service method to activate the account asynchronously
                var result = await _review.ActivateEmployeeAccountAsync(empId);

                // Return a response based on the success of the operation
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }

                return Json(new { success = true, message = "Employee activated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }


        //[HttpGet("Review/GetDeclarationDetails/{EmpId?}")]
        //public async Task<IActionResult> GetDeclarationDetails(int EmpId)
        //{
        //    // Use TransactionScope to manage a transaction across multiple operations
        //    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        try
        //        {
        //            // Fetch the result from the service layer
        //            var result = await _review.GetDeclarationDataByEmpIdAsync(EmpId);

        //            // Check if the result is successful and contains valid data
        //            if (result.Success && result.Data != null)
        //            {
        //                // Assuming result.Data is a single employee object and not a list
        //                var employeeData = result.Data as dynamic;

        //                if (employeeData != null)
        //                {
        //                    // Create a new EmployeeCredential entity
        //                    var employeeCredential = new EmployeesDeclaration
        //                    {
        //                        EmpId = employeeData[0].EmpId, // Assuming EmpId is part of the employee data
        //                        //Email = employeeData[0].EmailAddress,
        //                        //TempPassword = employeeData[0].Password,
        //                        //FirstName = employeeData[0].FirstName,
        //                        //LastName = employeeData[0].LastName,
        //                        //RoleId = employeeData[0].RoleId,
        //                        //LoginId = employeeData[0].LoginId,
        //                    };

        //                    //// Save to the EmployeesCred table
        //                    //var employementdetailRepository = _UnitOfWork.GetRepository<EmployeesCred>();
        //                    //await employementdetailRepository.AddAsync(employeeCredential);
        //                    //await _UnitOfWork.SaveAsync();

        //                    // After saving employee data, update IsProfileCompleted in EmployeeDetails table
        //                    var employeeDetailsRepository = _UnitOfWork.GetRepository<EmployeesDeclaration>();

        //                    // Find the EmployeeDetails record for the given EmpId
        //                    var employeeDetails = await employeeDetailsRepository.GetEmployeeDetailsByIdAsync(EmpId);

        //                    if (employeeDetails != null)
        //                    {
        //                        // Set IsProfileCompleted to true
        //                        employeeDetails[0].IsProfileCompleted = true;

        //                        // Save changes to the EmployeeDetails table
        //                        await _UnitOfWork.SaveAsync();
        //                    }
        //                    else
        //                    {
        //                        // Handle the case where EmployeeDetails is not found
        //                        throw new Exception("EmployeeDetails not found.");
        //                    }

        //                    // If everything is successful, commit the transaction
        //                    transaction.Complete();

        //                    // Return success response
        //                    return View("employeeview");
        //                }
        //                else
        //                {
        //                    return View("employeeview");
        //                }
        //            }
        //            else
        //            {
        //                // Handle the case where the service failed
        //                return View("employeeview");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle the error (e.g., log the exception)
        //            return Json(new { Success = false, Message = ex.Message });
        //        }
        //    }
        //}
    }
}

