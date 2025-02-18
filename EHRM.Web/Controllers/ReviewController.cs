using EHRM.DAL.Database;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.Review;
using EHRM.ServiceLayer.Utility;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Review;

using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _review;
        private readonly IEmailService _emailService;

        public ReviewController(IReviewService review, IEmailService emailService)
        {

            _review = review;
            _emailService = emailService;
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
                var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
                var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
                var name = userDetails.userName;
                var managerId = userDetails.userId;
                ViewData["name"] = name;
                ViewData["managerId"] = managerId;

                var result = await _review.GetAllProbationDataAsync();

                if (result.Success && result.Data != null)
                {
                    // Attempt to cast result.Data to IEnumerable<Team>
                    if (result.Data is IEnumerable<ProbationEvaluationQuestion> ques)
                    {
                        // Project the team list to a simplified JSON-friendly format
                        var questions = ques.Select(q => new EvaluationQuestion
                        {
                            QuestionId = q.QuestionId,
                            Question = q.Question
                        }).ToList();

                        EvaluationQuestion evaluationQuestion = new EvaluationQuestion();
                        evaluationQuestion.Items = questions;

                        return View(evaluationQuestion);
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
                        Question = probation.Question

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
                var result = await _review.UpdateQuestionDetailsAsync(id, model);

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


        #region Probation Evaluation Form

        [HttpGet]
        public async Task<IActionResult> GetEmployeeDetailsByManager()
        {
            // Fetch the probation data
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            var managerId = userDetails.userId;
            try
            {


                var result = await _review.GetEmployeeDetailsByManagerIdAsync(Convert.ToInt32(managerId));

                //ViewData["empId"] = managerId;

                //return Json(result);
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, massage = "error" });
            }

        }
        [HttpPost]
        public async Task<IActionResult> SaveEvaluationForm(EvaluationQuestion model)
        {
            var result = await _review.CreateEvaluationFormAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";

                EmailServiceModel _email = new()
                {
                    RecipentMail = "rajan.singh@rapscorp.com",  // Replace with actual recipient email
                    CcMail = "saksham@rapscorp.com",  // Replace with actual CC email
                    Subject = "test-email",
                    Body = "Probation Evaluation Form is submitted successfully !!"
                };

                // Sending the email
                _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);
                return RedirectToAction("ProbationEvaluation"); // Or whatever action you want to redirect to after a successful save
            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("ProbationEvaluation"); // Redirect back to the EmployeeType view
            }

        }


        #endregion


        #region Probation Dashboard

        [HttpGet]
        public async Task<JsonResult> GetAllDetails()
        {
            try
            {
                // Call the service method to get evaluation details
                var evaluationDetails = await _review.GetAllEvaluationDetails();

                // Check if the result is null or empty
                if (evaluationDetails == null || evaluationDetails.Count == 0)
                {
                    // Return JSON indicating failure and no data found
                    return Json(new { Success = false, Message = "No evaluation details found." });
                }

                // Project the evaluation details to a simplified JSON-friendly format
                var resList = evaluationDetails.Select(e => new
                {
                    id = e.Id,
                    recommendation = e.Recommendation,
                    remarksConfirmation = e.RemarksConfirmation,
                    firstName = e.Details.FirstName,
                    lastName = e.Details.LastName
                }).ToList();

                // Return the JSON response indicating success with data
                return Json(new { Success = true, Data = resList });
            }
            catch (Exception ex)
            {
                // Log the exception (you can add logging here) and return a JSON error response
                return Json(new { Success = false, Message = $"Internal server error: {ex.Message}" });
            }
        }
        #endregion
    }
}

