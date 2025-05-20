using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Master;
using EHRM.ServiceLayer.Setting;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Setting;
using EHRM.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingService _setting;
        private readonly string _fileStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public SettingsController(ISettingService setting, IConfiguration configuration, IUnitOfWork unitOfWork)
        {

            _setting = setting;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region Customize Login

        public async Task <IActionResult> CustomizeLogin()
        {
            var repository = _unitOfWork.GetRepository<CustomizeLogin>();
            var allData = await repository.GetAllAsync();
            var existingData = allData.FirstOrDefault();

            var viewModel = new CustomizeSettingViewModel();

            if (existingData != null)
            {
                viewModel.Id = existingData.Id;
                viewModel.OrganizationName = existingData.OrganizationName;
                viewModel.Bio = existingData.Bio;
                viewModel.ExistingLogoPath = existingData.LogoPath;
                viewModel.ExistingFaviconPath = existingData.FaviconPath;
            }

            return View(viewModel);
        
        }
        public async Task<IActionResult> CreateCustomLogin(CustomizeSettingViewModel model)
        {
            if (model.Id == 0) // This is for creating a new record
            {
                try
                {
                    // Validate and upload logo
                    if (model.LogoFile != null)
                    {
                        if (!IsValidImage(model.LogoFile, 150))
                        {
                            TempData["ToastType"] = "danger";
                            TempData["ToastMessage"] = "Logo must be a PNG, JPG, or JPEG and ≤ 150KB.";
                            return RedirectToAction("CustomizeLogin");
                        }
                        model.ExistingLogoPath = Upload(model.LogoFile); // Upload logo
                    }

                    // Validate and upload favicon
                    if (model.FaviconFile != null)
                    {
                        if (!IsValidImage(model.FaviconFile, 50))
                        {
                            TempData["ToastType"] = "danger";
                            TempData["ToastMessage"] = "Favicon must be a PNG, JPG, or JPEG and ≤ 50KB.";
                            return RedirectToAction("CustomizeLogin");
                        }
                        model.ExistingFaviconPath = Upload(model.FaviconFile); // Upload favicon
                    }

                    var result = await _setting.CreateCustomLoginAsync(model);

                    if (result.Success)
                    {
                        TempData["ToastType"] = "success";
                        TempData["ToastMessage"] = "Record has been created";
                        return RedirectToAction("CustomizeLogin");
                    }
                    else
                    {
                        TempData["ToastType"] = "danger";
                        TempData["ToastMessage"] = "An error occurred while creating the record.";
                        return RedirectToAction("CustomizeLogin");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ToastType"] = "danger";
                    TempData["ToastMessage"] = $"Error creating record: {ex.Message}";
                    return RedirectToAction("CustomizeLogin");
                }
            }
            else // This is for updating an existing record
            {
                try
                {
                    // Validate and upload logo if a new file is provided
                    if (model.LogoFile != null)
                    {
                        if (!IsValidImage(model.LogoFile, 150))
                        {
                            TempData["ToastType"] = "danger";
                            TempData["ToastMessage"] = "Logo must be a PNG, JPG, or JPEG and ≤ 150KB.";
                            return View(model);
                        }
                        model.ExistingLogoPath = Upload(model.LogoFile); // Upload new logo if provided
                    }

                    // Validate and upload favicon if a new file is provided
                    if (model.FaviconFile != null)
                    {
                        if (!IsValidImage(model.FaviconFile, 50))
                        {
                            TempData["ToastType"] = "danger";
                            TempData["ToastMessage"] = "Favicon must be a PNG, JPG, or JPEG and ≤ 50KB.";
                            return View(model);
                        }
                        model.ExistingFaviconPath = Upload(model.FaviconFile); // Upload new favicon if provided
                    }

                    var updateResult = await UpdateCustomLogin(model.Id, model);

                    if (updateResult != null)
                    {
                        var updateResponse = updateResult as dynamic;

                        if (updateResponse?.success == true)
                        {
                            TempData["ToastType"] = "success";
                            TempData["ToastMessage"] = "Record has been updated";
                            return RedirectToAction("CustomizeLogin");
                        }
                        else
                        {
                            TempData["ToastType"] = "danger";
                            TempData["ToastMessage"] = "An error occurred while updating the record.";
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["ToastType"] = "danger";
                        TempData["ToastMessage"] = "Unexpected error occurred during update.";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    TempData["ToastType"] = "danger";
                    TempData["ToastMessage"] = $"Error updating record: {ex.Message}";
                    return View(model);
                }
            }

        }



        private string Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string path;
            if (_configuration["AppSetting:EnvironmentName"] == "Production")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");
            }
            else
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("\\Files", fileName); // Web-friendly path
        }

        private bool IsValidImage(IFormFile file, int maxSizeKB)
        {
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return false;

            if (file.Length > maxSizeKB * 1024)
                return false;

            return true;
        }

        [HttpGet]
        public async Task<JsonResult> GetLatestCustomLogin(int id)
        {
            try
            {
                var result = await _setting.GetCustomLoginByIdAsync(id);

                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the latest custom login." });
            }
        }

        [NonAction]
        private async Task<object> UpdateCustomLogin(int id, CustomizeSettingViewModel model)
        {
            try
            {
                int updatedBy = 1;
                // Call the service method to update the role
                var result = await _setting.UpdateCustomLoginAsync(id, model);

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

        #endregion

    }
}
