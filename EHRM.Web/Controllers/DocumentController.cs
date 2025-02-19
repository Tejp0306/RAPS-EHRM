using EHRM.DAL.Database;
using EHRM.ServiceLayer.Calendar;
using EHRM.ServiceLayer.Document;
using EHRM.ViewModel.Document;
using EHRM.ViewModel.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace EHRM.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _document;

        public DocumentController(IDocumentService document)
        {

            _document = document;
        }

        public string FilePath { get; private set; }

        public IActionResult AddDocument()
        {
            return View();
        }

        [NonAction]
        private async Task<object> UpdateDocument(int id, DocumentViewModel model)
        {
            try
            {
                int updatedBy = 1;
                // Call the service method to update the role

                if (model.File != null)
                {
                    FilePath = Upload(model); // Assume Upload is a method that handles file uploads
                }
                var result = await _document.UpdateDocumentAsync(id, updatedBy, model, FilePath);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = "Document Updated"
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
        public async Task<IActionResult> UploadDocument(DocumentViewModel model)
        {


            if (model.DocumentId == 0)
            {
                try
                {
                    // Check if a file is included in the request
                    if (model.File != null)
                    {
                        FilePath = Upload(model); // Assume Upload is a method that handles file uploads
                    }

                    string createdById = "waseem"; // Replace with logic to fetch the actual user ID
                    var result = await _document.SaveDocumentAsync(model, createdById, FilePath);

                    // Handle the result of the create operation
                    if (result.Success)
                    {
                        TempData["ToastType"] = "success";  // Success, danger, warning, info
                        TempData["ToastMessage"] = "Operation completed successfully!";
                        return RedirectToAction("AddDocument"); // Redirect to the list of roles
                    }
                    else
                    {
                        ViewBag.ErrorMessage = result.Message; // Display error message
                        return View(model); // Return to the same view with the provided model
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "An error occurred while uploading the document.");
                }
            }
            else
            {
                var updateResult = await UpdateDocument(model.DocumentId, model);

                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        // Success: Store success message and toast
                        TempData["ToastType"] = "success"; // Success toast type
                        TempData["ToastMessage"] = "Document has been updated"; // Success message
                        return RedirectToAction("AddDocument"); // Redirect to the list of notices
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

            // Return the same view with validation errors if any
            return View(model);


        }

        // Get document table data
        [HttpGet]
        public async Task<JsonResult> GetAllDocumentData()

        {
            // Fetch the result from the service layer
            var result = await _document.GetAllDocumentAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                // The data is already a list of anonymous types with TeamName included
                var document = result.Data as IEnumerable<dynamic>;
                if (document != null)
                {
                    // Use Select to map the holidays to the desired output format
                    var documentList = document.Select(doc => new
                    {
                        DocumentId = doc.DocumentId,
                        DocumentType = doc.DocumentType,
                        Description=doc.Description,
                        UploadedAt= doc.UploadedAt

                     
                    }).ToList(); // Convert to a List

                    return Json(documentList);
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


        [HttpGet]
        public async Task<JsonResult> GetDocumentDetails(int id)
        {
            try
            {
                var nb = await _document.GetAllDocumentByIdAsync(id);

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


        // SHow Document

        [HttpGet]
        public async Task<IActionResult> ShowFile(int id)
        {
            try
            {
                var nb = await _document.GetAllDocumentByIdAsync(id);

                if (nb == null)
                {
                    return Json(new { success = false, message = "Notice not found." });
                }


                return Json(new { success = true, data = nb });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Document." });
            }
        }


        private string Upload(DocumentViewModel model)
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
            return Path.Combine("\\Files", fileName);
        }

    }
}