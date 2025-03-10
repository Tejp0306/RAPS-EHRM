using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Enumerations;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.LeaveDashBoard;
using EHRM.ServiceLayer.Utility;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Leave;
using EHRM.ViewModel.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Transactions;

namespace EHRM.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employee;
        private readonly ILeaveDashboardService _leaveDashboardService;
        private readonly EhrmContext _context;
        private readonly IUnitOfWork _UnitOfWork;

        
        private readonly IEmailService _emailService;
        public EmployeeController(IEmployeeService employee, EhrmContext context, IEmailService emailService, IUnitOfWork unitOfWork, ILeaveDashboardService leaveDashboardService )
        {
            _employee = employee;
            _context = context; 
            _emailService = emailService;
            _UnitOfWork = unitOfWork;
            _leaveDashboardService = leaveDashboardService;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("Employee/AddEmployee/{EmpId?}")]
        public async Task<IActionResult> AddEmployee(int? EmpId = null)
            {
            // Dropdowns for employee type and employment status
            var EmployeeTypes = Enum.GetValues(typeof(EmployeeType))
                                    .Cast<EmployeeType>()
                                    .Select(e => new SelectListItem
                                    {
                                        Value = ((int)e).ToString(),
                                        Text = e.ToString()
                                    })
                                    .ToList();

            var EmploymentStatuses = Enum.GetValues(typeof(EmploymentStatus))
                                         .Cast<EmploymentStatus>()
                                         .Select(e => new SelectListItem
                                         {
                                             Value = ((int)e).ToString(),
                                             Text = e.ToString()
                                         })
                                         .ToList();

            ViewBag.EmployeeType = EmployeeTypes;
            ViewBag.EmploymentStatusId = EmploymentStatuses;

            if (EmpId.HasValue)
                {
                // Fetch the existing employee details
                var employee = await _employee.GetAllEmployeeRecordDetails(EmpId.Value);
                HttpContext.Session.SetInt32("EmpId", (int)EmpId);
                if (employee != null)
                {
                    return View(employee.FirstOrDefault());
                }
            }

            // Return an empty model for new employee creation
            return View(new GetAllEmployeeViewModel());
        }

        

        public IActionResult employeeview()
        {
            return View();
        }


        [NonAction]
        private async Task<object> UpdatePersonalDetails(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _employee.UpdatePersonalInfoAsync(id, updatedBy, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception ex)
            {
                
                return new
                {
                    success = false,
                    message = "An error occurred while updating the role. Please try again later."
                };
            }
        }

        //Save Personal Info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePersonalInfo(GetAllEmployeeViewModel model)
        {
            if (model == null)
            {
                TempData["ToastType"] = "danger";
                TempData["ToastMessage"] = "Invalid data submitted.";
                return RedirectToAction("AddEmployee");
            }

            if (_employee.CheckUserInDbByEmpId(model.EmpId))
            {
                // Update existing employee details
                string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdatePersonalDetails((int)model.EmpId, updatedBy, model);

                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success";
                        TempData["ToastMessage"] = "Record has been updated successfully!";
                        return RedirectToAction("AddEmployee");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse?.message ?? "An error occurred while updating the record.";
                        return View(model); // Return to the same view with validation errors
                    }
                }

                TempData["ToastType"] = "danger";
                TempData["ToastMessage"] = "An error occurred while updating the record.";
                return RedirectToAction("AddEmployee");
            }
            else
            {
                // Handle new employee creation
                if (model.ProfileImg == null)
                {
                    TempData["ToastType"] = "warning";
                    TempData["ToastMessage"] = "Profile image is mandatory!";
                    return RedirectToAction("AddEmployee");
                }

                // Upload the profile image
                string filepath = Upload(model);

                if (string.IsNullOrEmpty(filepath))
                {
                    TempData["ToastType"] = "danger";
                    TempData["ToastMessage"] = "An error occurred while uploading the profile image.";
                    return RedirectToAction("AddEmployee");
                }

                // Save new employee details
                int createdById = 101; // Replace with actual logic to fetch the current user ID
                var result = await _employee.SavePersonalInfoAsync(model, createdById, filepath);

                if (result?.Success == true)
                {
                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Personal Info Saved uccessfully!";

                    if (int.TryParse(result?.Data?.ToString(), out int empId))
                    {
                        HttpContext.Session.SetInt32("EmpId", empId);
                        TempData["EmpId"] = empId;
                    }

                    return RedirectToAction("AddEmployee");
                }
                else
                {
                    ViewBag.ErrorMessage = result?.Message ?? "An error occurred while saving the record.";
                    return View(model);
                }
            }
        }



        //Update Employment Info

        [NonAction]
        private async Task<object> UpdateEmploymentInfoDetails(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _employee.UpdateEmploymentInfoAsync(id, updatedBy, model);


                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Recode  Saved successfully!";
                var sessionEmpId = result?.Data?.ToString();
                if (!string.IsNullOrEmpty(sessionEmpId) && int.TryParse(sessionEmpId, out int empId))
                {
                    HttpContext.Session.SetInt32("EmpId", empId);
                }

                return result;

                
            }
            catch (Exception ex)
            {

                return new
                {
                    success = false,
                    message = "An error occurred while updating the role. Please try again later."
                };
            }
        }



        //Save Employement Info
        [HttpPost]
        public async Task<IActionResult> SaveEmploymentInfo(GetAllEmployeeViewModel model)
        {
            int sessionempid = (int)model.EmploymentDetails.EmpId;

            // Check if the employee exists in the database by EmpId
            if (_employee.CheckUserInEmploymentDbByEmpId(sessionempid))
            {
                // Update Employment Info
                string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateEmploymentInfoDetails(sessionempid, updatedBy, model);

                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse.Success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record has been updated successfully!";
                    }
                    else
                    {
                        TempData["ToastType"] = "danger"; // Store error message
                        TempData["ToastMessage"] = updateResponse?.message ?? "An error occurred while updating.";
                    }

                }
                else
                {
                    TempData["ToastType"] = "danger"; // Store error message
                    TempData["ToastMessage"] = "Failed to update the record.";
            }

                return RedirectToAction("AddEmployee"); // Redirect after update operation
            }
            else
            {
                // Create Employment Info as the employee does not exist in the database
                int createdById = 101; // Replace with logic to fetch the actual user ID
                var result = await _employee.SaveEmploymentInfoAsync(model, createdById);

              

                // Handle the result of the create operation
                if (result.EmpId>0)
                {
                    var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
                    var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
                    int empId = (int)result.EmpId;

                    // Retrieve employee employment details
                    var employementDetailsRepository = _UnitOfWork.GetRepository<EmployementTypeDetail>();
                    var employeeDetails = await employementDetailsRepository.GetEmployementTypeDetailsByIdAsync(empId);

                    // Check if employee details exist and are not empty
                    if (employeeDetails == null || !employeeDetails.Any())
                    {
                        return NotFound("Employee details not found.");
                    }

                    // Ensure StartDate is valid before proceeding
                    if (!DateTime.TryParse(employeeDetails[0].StartDate, out DateTime startDate))
                    {
                        return BadRequest("Invalid StartDate format.");
                    }

                    // Calculate leave summary based on the employee's StartDate
                    var leaveSummary = await _leaveDashboardService.CalculateLeavePolicy(startDate);

                    var newemployeeLeaveBalance = new LeaveBalance

                {
                        EmpId = (int)result.EmpId,
                        TenureYears = leaveSummary.Tenure,
                        EarnedLeave =leaveSummary.EarnedLeave,
                        SickLeave = leaveSummary.SickLeave,
                        CasualLeave = leaveSummary.CasualLeave,
                        TotalLeave=leaveSummary.TotalLeave

                    };
                    var LeaveBalanceRepository = _UnitOfWork.GetRepository<LeaveBalance>();
                    await LeaveBalanceRepository.AddAsync(newemployeeLeaveBalance);
                    await _UnitOfWork.SaveAsync();

                    TempData["ToastType"] = "success"; // Success message
                    TempData["ToastMessage"] = "Operation completed successfully!";
                    //TempData["EmpId"] = result.Data; // Store the created EmpId
                }
                else
                {
                    TempData["ToastType"] = "danger"; // Error message
                    //TempData["ToastMessage"] = result.Message ?? "An error occurred while saving.";
                }

                return RedirectToAction("AddEmployee"); // Redirect after create operation
            }
            }
            
        // Update Qualification Info

        [NonAction]
        private async Task<object> UpdateQualificationInfoDetails(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _employee.UpdateQualificationInfoAsync(id, updatedBy, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception ex)
            {

                return new
                {
                    success = false,
                    message = "An error occurred while updating the role. Please try again later."
                };
            }
        }


        //Save Qualification Details
        [HttpPost]
        public async Task<IActionResult> SaveQualificationInfo(GetAllEmployeeViewModel model)
        {

            int empid = (int)model.Qualifications.EmpId;

            if (_employee.CheckUserInDbByEmpId(empid) == true)
            {

                // Update the role details
                string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateQualificationInfoDetails(empid, updatedBy, model);
                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record Has been updated ";
                        return RedirectToAction("AddEmployee"); // Redirect to the list of roles
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse?.message; // Display error message
                        return View(model); // Return to the same view with the provided model
                    }

                }

                return RedirectToAction("AddEmployee");
            }

            else
            {
                int createdById = 101; // Replace with logic to fetch the actual user ID
                var result = await _employee.SaveQualificationInfoAsync(model, createdById);
                // Handle the result of the create operation
                if (result.Success)
                {

                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Operation completed successfully!";
                    TempData["EmpId"] = result.Data;
                    return RedirectToAction("AddEmployee"); // Redirect to the list of roles
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message; // Display error message
                    return RedirectToAction("AddEmployee");// Return to the same view with the provided model
                }
            }      
        }

        // Update Salary Info

        [NonAction]
        private async Task<object> UpdateSalaryInfoDetails(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _employee.UpdateSalaryInfoAsync(id, updatedBy, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception ex)
            {

                return new
                {
                    success = false,
                    message = "An error occurred while updating the role. Please try again later."
                };
            }
        }


        //Save Salary Info

        [HttpPost]
        public async Task<IActionResult> SaveSalaryInfo(GetAllEmployeeViewModel model)
        {
            int empid = (int)model.SalaryDetails.EmpId;

            if (_employee.CheckUserInDbByEmpId(empid) == true)
            {

                // Update the role details
                string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateSalaryInfoDetails(empid, updatedBy, model);
                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record Has been updated ";
                        return RedirectToAction("AddEmployee"); // Redirect to the list of roles
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse?.message; // Display error message
                        return View(model); // Return to the same view with the provided model
                    }

                }

                return RedirectToAction("AddEmployee");
            }

            else
            {
                int createdById = 101; // Replace with logic to fetch the actual user ID
                var result = await _employee.SaveSalaryInfoAsync(model, createdById);
                // Handle the result of the create operation
                if (result.Success)
                {

                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Operation completed successfully!";
                    TempData["EmpId"] = result.Data;
                    return RedirectToAction("AddEmployee"); // Redirect to the list of roles
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message; // Display error message
                    return RedirectToAction("AddEmployee");// Return to the same view with the provided model
                }
            }

        }

        // Update Declaration Form

        [NonAction]
        private async Task<object> UpdateDeclarationInfoDetails(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _employee.UpdateDeclarationInfoAsync(id, updatedBy, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception ex)
            {

                return new
                {
                    success = false,
                    message = "An error occurred while updating the role. Please try again later."
                };
            }
        }


        //Save Declaration Form

        [HttpPost]
        public async Task<IActionResult> SaveDeclarationInfo(GetAllEmployeeViewModel model)
        {

            int empid = (int)model.Declarations.EmpId;

            if (_employee.CheckUserInDbByEmpId(empid) == true)
            {

                // Update the role details
                string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateDeclarationInfoDetails(empid, updatedBy, model);
                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record Has been updated ";
                        return RedirectToAction("AddEmployee"); // Redirect to the list of roles
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse?.message; // Display error message
                        return View(model); // Return to the same view with the provided model
                    }

                }

                return RedirectToAction("AddEmployee");
            }

            else
            {
                int createdById = 101; // Replace with logic to fetch the actual user ID
                var result = await _employee.SaveDecalarationInfoAsync(model, createdById);
                // Handle the result of the create operation
                if (result.Success)
                {

                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Operation completed successfully!";
                    TempData["EmpId"] = result.Data;
                    return RedirectToAction("employeeview"); // Redirect to the list of roles
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message; // Display error message
                    return RedirectToAction("AddEmployee");// Return to the same view with the provided model
                }

            }
        }

        [HttpGet]
        public async Task<JsonResult> GetRole()
        {
            // Fetch the result from the service layer
            var result = await _employee.GetRoleAsync();

            if (result.Success && result.Data != null)
            {
                // Attempt to cast result.Data to IEnumerable<Team>
                if (result.Data is IEnumerable<Role> role)
                {
                    // Project the team list to a simplified JSON-friendly format
                    var roleList = role.Select(role => new
                    {
                        id = role.RoleId, // Ensure Team class has an Id property
                        name = role.RoleName
                    }).ToList();

                    return Json(new { Success = true, Data = roleList });
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format (IEnumerable<Role>)." });
                }
            }
            else
            {
                // Handle failure scenarios
                return Json(new { Success = false, Message = result.Message ?? "No Roles found." });
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetManager()
        {
            // Fetch the result from the service layer
            var result = await _employee.GetManagerAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                if (result.Data is IEnumerable<GetAllEmployeeViewModel> employeeList)
                {
                    // Project the employee list to a simplified JSON-friendly format
                    var resList = employeeList.Select(e => new
                    {
                        empId = e.EmpId,
                        name = e.FirstName + " " + e.LastName // Concatenate first and last names
                    }).ToList();

                    // Return the JSON response
                    return Json(new { Success = true, Data = resList });
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format (IEnumerable<GetAllEmployeeViewModel>)." });
                }
            }
            else
            {
                // Handle failure scenarios
                return Json(new { Success = false, Message = result.Message ?? "No employees found." });
            }
        }
        private string Upload(GetAllEmployeeViewModel model)
        {
            // Check if a file is provided, if not, simply return null (indicating no file upload)
            if (model.ProfileImg == null || model.ProfileImg.Length == 0)
            {
                return null; // No file uploaded, return null or an empty string
            }

            // Define the directory path to store files
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProfileImage");


            // Create the folder if it doesn't exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Generate a unique file name to avoid name conflicts (optional, can use the original name)
            FileInfo fileInfo = new FileInfo(model.ProfileImg.FileName);
            string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;  // Unique file name generation
                                                                               // You can also use model.FileName here if you want to allow users to specify the name

            // Combine path with the file name to get the full file path
            string fileNameWithPath = Path.Combine(path, fileName);

            // Save the file to the specified directory
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.ProfileImg.CopyTo(stream);
            }

            // Return the full file path or file name
            return Path.Combine("\\ProfileImage", fileName);
        }
        public async Task<JsonResult> GetEmployeeData()
        {
            // Fetch the result from the service layer
            var result = await _employee.GetEmployeeDataAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                // The data is already a list of anonymous types with TeamName included
                var Employee = result.Data as IEnumerable<dynamic>;
                if (Employee != null)
                {
                    // Use Select to map the holidays to the desired output format
                    var EmployeeList = Employee.Select(emp => new
                    {
                        Id = emp.EmpId,
                        Name = emp.FirstName + ' ' + emp.LastName,
                        Email = emp.EmailAddress,
                        ProfileStatus = emp.IsProfileCompleted,
                        EmploymentStatus = emp.Active,
                        

                    }).ToList(); // Convert to a List


                    return Json(EmployeeList);

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

        //Get Employee Data for saving data in employee cred
        [HttpGet("Employee/GetEmployeeForCred/{EmpId?}")]
        public async Task<IActionResult> GetEmployeeForCred(int EmpId)
        {
            try
            {
                // Check if the employee already exists in the credential database
                if (_employee.CheckUserInEmpCredDbByEmpId(EmpId))
                {
                    // Update IsProfileCompleted to false in EmployeeDetails table
                    var employeeDetailsRepository = _UnitOfWork.GetRepository<EmployeeDetail>();
                    var employeeDetails = await employeeDetailsRepository.GetEmployeeDetailsByIdAsync(EmpId);
                        
                    var employmentDetailsRepository = _UnitOfWork.GetRepository<EmployeesCred>();
                    var employmentDetails = await employmentDetailsRepository.GetEmployeeCredByIdAsync(EmpId);

                    if (employeeDetails?.Any() == true && employmentDetails?.Any() == true)
                    {
                        employeeDetails[0].Active = false;
                        await _UnitOfWork.SaveAsync();

                        employmentDetails[0].Active = false;
                        await _UnitOfWork.SaveAsync();

                        // Add a success message to TempData
                        TempData["Message"] = "Profile marked as incomplete.";
                        TempData["MessageType"] = "success"; // You can set the type to 'error' for error messages

                        return RedirectToAction("Employeeview");
                    }
                    else
                    {
                        // Handle the case where EmployeeDetails is not found
                        TempData["Message"] = "Employee details not found.";
                        TempData["MessageType"] = "error"; // Show error toast

                        return RedirectToAction("Employeeview");
                    }
                }
                else
                {
                    // Use TransactionScope for transactional integrity
                    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        try
                        {
                            // Fetch the result from the service layer
                            var result = await _employee.GetEmployeeDataByEmpIdAsync(EmpId);

                            // Check if the result is successful and contains valid data
                            if (result.Success && result.Data != null)
                            {
                                var employeeData = result.Data as dynamic;

                                if (employeeData != null && employeeData.Count > 0)
                                {
                                    // Create a new EmployeeCredential entity
                                    var employeeCredential = new EmployeesCred
                                    {
                                        EmpId = employeeData[0].EmpId,
                                        Email = employeeData[0].EmailAddress,
                                        TempPassword = employeeData[0].Password,
                                        FirstName = employeeData[0].FirstName,
                                        LastName = employeeData[0].LastName,
                                        RoleId = employeeData[0].RoleId,
                                        LoginId = employeeData[0].LoginId,
                                        Active = true,
                                    };

                                    // Save to the EmployeesCred table
                                    var employmentDetailRepository = _UnitOfWork.GetRepository<EmployeesCred>();
                                    await employmentDetailRepository.AddAsync(employeeCredential);
                                    await _UnitOfWork.SaveAsync();

                                    // Update IsProfileCompleted in EmployeeDetails table
                                    var employeeDetailsRepository = _UnitOfWork.GetRepository<EmployeeDetail>();
                                    var employeeDetails = await employeeDetailsRepository.GetEmployeeDetailsByIdAsync(EmpId);

                                    if (employeeDetails != null && employeeDetails.Any())
                                    {
                                        //employeeDetails[0].IsProfileCompleted = true;
                                        employeeDetails[0].Active = true;
                                        await _UnitOfWork.SaveAsync();

                                        


                                    }
                                    else
                                    {
                                        // Handle case where EmployeeDetails is not found
                                        throw new Exception("Employee details not found.");
                                    }

                                    EmailServiceModel _email = new()

                                    {

                                        RecipentMail = employeeData[0].EmailAddress,  // Replace with actual recipient email

                                        CcMail = "arjun@rapscorp.com",  // Replace with actual CC email

                                        Subject = "Welcome to Raps Consulting Inc – Your Account Details",

                                        Body = @"
                                                    <html>
                                                    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                                                    <p>Dear <strong>" + employeeData[0].FirstName + @"</strong>,</p>
 
                                                                <p>Welcome to <strong>Raps Consulting Inc</strong>! We are delighted to have you on board.</p>
 
                                                                <p>Your EHRMS Portal account has been successfully created. Below are your login details:</p>
 
                                                                <p><strong>Username:</strong> " + employeeData[0].EmailAddress + @"<br>
                                                    <strong>Temporary Password:</strong> " + employeeData[0].Password + @"</p>
 
                                                                <p>To ensure the security of your account, we recommend changing your password upon your first login.</p>
 
                                                                <p>You can access your account by clicking the link below:</p>
 
                                                                <p><a href='https://ehrm.rapsit.com/' style='color: #007BFF; font-weight: bold;'>Login to Your Account</a></p>
 
                                                                <p>If you have any questions or need assistance, feel free to reach out to our support team at 
                                                    <a href='mailto:support@rapscorp.com' style='color: #007BFF;'>support@rapscorp.com</a>.</p>
 
                                                                <p>We look forward to working with you!</p>
 
                                                                <p>Best regards,</p>
                                                    <p><strong>HR Department</strong><br>
                                                    <strong>Raps Consulting Inc</strong><br>
                                                    <a href='https://www.rapscorp.com/' style='color: #007BFF;'>https://www.rapscorp.com/</a></p>
                                                    </body>
                                                    </html>"

                                    };



                                    // Sending the email
                                    _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);
                                    //return RedirectToAction("EmployeeProfile"); // Redirect to the list of employee types

                                    // Commit the transaction
                                    transaction.Complete();

                                    // Add success message to TempData
                                    TempData["Message"] = "Employee data processed successfully.";
                                    TempData["MessageType"] = "success";

                                    return RedirectToAction("Employeeview");
                                }
                                else
                                {
                                    // Handle invalid or empty employee data
                                    TempData["Message"] = "Invalid employee data.";
                                    TempData["MessageType"] = "error"; // Show error toast

                                    return RedirectToAction("Employeeview");
                                }
                            }
                            else
                            {
                                // Handle service failure or no data found
                                TempData["Message"] = "Employee data not found or service failed.";
                                TempData["MessageType"] = "error";

                                return RedirectToAction("Employeeview");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle the error and rollback the transaction
                            TempData["Message"] = ex.Message;
                            TempData["MessageType"] = "error";

                            return RedirectToAction("Employeeview");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any outer exceptions
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "error";

                return RedirectToAction("Employeeview");
            }
        }




        // Non-action method to check profile completion status
        //[NonAction]
        //private string CheckProfileStatus(bool isProfileCompleted)
        //{
        //    if (!isProfileCompleted)
        //    {
        //        return "Profile Incomplete";
        //    }

        //    return "Profile Complete";
        //}
        public async Task<JsonResult> GetTeamData()
        {
            // Fetch the result from the service layer
            var result = await _employee.GetTeamAsync();

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
        [HttpPost]
        public async Task<JsonResult> GetAge(string dateBirth)
        {
            if (DateTime.TryParse(dateBirth, out DateTime dob))
            {
                var today = DateTime.Today;
                var age = today.Year - dob.Year;

                // Adjust if the birthday has not occurred yet this year
                if (dob.Date > today.AddYears(-age))
                {
                    age--;
                }
                return Json(new { success = true, age });
            }
            return Json(new { success = false, message = "Invalid date format" });
        }
        [HttpPost]
        public async Task<JsonResult> GenerateLogin(string firstName)
        {
            // Get the current year
            int currentYear = DateTime.Now.Year;

            // Generate the LoginId by appending the current year to the first name
            string loginId = $"{firstName.ToLower()}.{currentYear}";
            string password = $"{firstName.ToLower()}{currentYear}@" + GenerateRandomPassword();
            // Return the result as a JSON response
            return Json(new { success = true, loginId , password });
        }
        // Example of a helper method to generate a random password (you can customize the logic)
        [HttpPost]
        public async Task<JsonResult> CheckExistingEmpId(int empId)
        {
            // Check if the employee ID exists in the EmployeeDetails table
            bool isExist = await _context.EmployeeDetails.AnyAsync(x => x.EmpId == empId);

            // Return a flag: 1 if exists, 0 otherwise
            return Json(new { flag = isExist ? 1 : 0 });
        }

        public async Task<JsonResult> CheckExistingEmail(string EmailAddress)
        {
            // Check if the employee ID exists in the EmployeeDetails table
            bool isExist = await _context.EmployeeDetails.AnyAsync(x => x.EmailAddress == EmailAddress);

            // Return a flag: 1 if exists, 0 otherwise
            return Json(new { flag = isExist ? 1 : 0 });
        }

        private string GenerateRandomPassword(int length = 8)
        {
            var random = new Random();
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var password = new string(Enumerable.Repeat(validChars, length)
                                                .Select(s => s[random.Next(s.Length)]).ToArray());
            return password;
        }


        #region EmployeeDeclaration
        public IActionResult EmployeeDeclaration()
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
        public IActionResult EmployeeDeclarationone()
        {
            return View();
        }
        public IActionResult EmployeeProfile(EmployeeDeclarationViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveDeclaration(EmployeeDeclarationViewModel model)
        {

            var result = await _employee.CreateDeclarationAsync(model);

            // Handle the result of the create operation
            if (result.Success)
            {
                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Record saved successfully!";

                EmailServiceModel _email = new()
                {
                    RecipentMail = "waseem@rapscorp.com",  // Replace with actual recipient email
                    CcMail = "saksham@rapscorp.com",  // Replace with actual CC email
                    Subject = "test-email",
                    Body = "Employee-declaration for is submitted successfully !!, Thanks,"
                };

                // Sending the email
                _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);
                return RedirectToAction("EmployeeProfile"); // Redirect to the list of employee types
            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while submitting the form.";
                return RedirectToAction("EmployeeDeclaration"); // Redirect back to the EmployeeType view
            }
        }

        public async Task<IActionResult> GetEmployeeProfileDataByIDDOB(EmployeeDeclarationViewModel model)
        {
            // Fetch the result from the service layer

            //string Parseddob = DateTime.ParseExact(model.DateOfBirth, "dd - MMMM - yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            DateTime updatedDateData = Convert.ToDateTime(model.DateOfBirth);
            //string formattedDate = updatedDateData.ToString("dd/MM/yyyy");

            var result = await _employee.GetEmployeeDetailsByEmpIdDOB(model.EmpId, updatedDateData);

            if (result.Count > 0)
            {
                TempData["ToastType"] = "success"; // Store success message
                TempData["ToastMessage"] = "User Found";

                EmployeeDeclarationViewModel employee = new()
                {
                    EmpId = (int)result[0].EmpId,
                    DateOfBirth = Convert.ToDateTime(result[0].DateOfBirth).ToString("dd/MM/yyyy"),

                };

                return View("EmployeeProfile", employee);
            }
            else
            {
                model.EmpId = 0;

                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "User may deleted or removed";

                return View("EmployeeProfile", model);
            }
        }

        public async Task<IActionResult> EmployeeProfileDetails(int id)
        {
            // Fetch the result from the service layer
            var result = await _employee.GetAllEmployeeProfileDetails(id);

            if (result.Count > 0)
            {
                TempData["ToastType"] = "success"; // Store success message
                TempData["ToastMessage"] = "User Found";
                var Model = result[0];

                return View("EmployeeDeclarationone", Model);
            }
            else
            {
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "User may deleted or removed";

                return View("EmployeeDeclarationone");
            }
            //return View();
        }

        #endregion

       
    }

}
