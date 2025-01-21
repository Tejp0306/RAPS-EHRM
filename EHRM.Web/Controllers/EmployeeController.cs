using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Enumerations;
using EHRM.ServiceLayer.Utility;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
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
        private readonly EhrmContext _context;
        private readonly IUnitOfWork _UnitOfWork;

        
        private readonly IEmailService _emailService;
        public EmployeeController(IEmployeeService employee, EhrmContext context, IEmailService emailService, IUnitOfWork unitOfWork )
        {
            _employee = employee;
            _context = context; 
            _emailService = emailService;
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Employee/AddEmployee/{EmpId?}")]
        public async Task<IActionResult> AddEmployee(int? EmpId = null)
        {
            

            var EmployeeTypes = Enum.GetValues(typeof(EmployeeType))
                                 .Cast<EmployeeType>()
                                 .Select(e => new SelectListItem
                                 {
                                     Value = ((int)e).ToString(),  // Integer value as string
                                     Text = e.ToString()           // Enum name as text
                                 })
                                 .ToList();


            var employmentStatuses = Enum.GetValues(typeof(EmploymentStatus))
                                 .Cast<EmploymentStatus>()
                                 .Select(e => new SelectListItem
                                 {
                                     Value = ((int)e).ToString(),  // Integer value as string
                                     Text = e.ToString()           // Enum name as text
                                 })
                                 .ToList();

            // Pass the list of SelectListItems to ViewBag
            ViewBag.EmployeeType = EmployeeTypes;
            ViewBag.EmploymentStatusId = employmentStatuses;

            if (EmpId.HasValue)
            {
                var res = await _employee.GetAllEmployeeRecordDetails(EmpId.Value);
                // Return the result model to the view if EmpId is provided
                return View(res);
            }
            else
            {
                // Return a default or empty model if EmpId is not provided
                var defaultModel = new List<GetAllEmployeeViewModel>();
                return View();
            }
        }
        public IActionResult employeeview()
        {
            return View();
        }

        //Save Personal Info
        [HttpPost]
        public async Task<IActionResult> SavePersonalInfo(GetAllEmployeeViewModel model)
        {
         
           var filepath = "";
            if (model.ProfileImg != null)
            {

                filepath = Upload(model);
            }
            else
            {
                TempData["ToastType"] = "warning";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Profile Image is Mandatory!";
                return RedirectToAction("AddEmployee"); // Redirect to the list of roles
            }
            int createdById = 101; // Replace with logic to fetch the actual user ID
            var result = await _employee.SavePersonalInfoAsync(model, createdById, filepath);
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
                return View(model); // Return to the same view with the provided model
            }
        }


        //Save Employement Info
        [HttpPost]
        public async Task<IActionResult> SaveEmploymentInfo(GetAllEmployeeViewModel model)
        {
            int createdById = 101; // Replace with logic to fetch the actual user ID
            var result = await _employee.SaveEmploymentInfoAsync(model, createdById);
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


        //Save Qualification Details
        [HttpPost]
        public async Task<IActionResult> SaveQualificationInfo(GetAllEmployeeViewModel model)
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

        //Save Salary Info
        

        [HttpPost]
        public async Task<IActionResult> SaveSalaryInfo(GetAllEmployeeViewModel model)
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


        //Save Declaration Form

        [HttpPost]
        public async Task<IActionResult> SaveDeclarationInfo(GetAllEmployeeViewModel model)
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
                    return Json(new { Success = false, Message = "Data is not in the expected format (IEnumerable<Team>)." });
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
                        ProfileStatus = CheckProfileStatus(emp.IsProfileCompleted)

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
            // Use TransactionScope to manage a transaction across multiple operations
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Fetch the result from the service layer
                    var result = await _employee.GetEmployeeDataByEmpIdAsync(EmpId);

                    // Check if the result is successful and contains valid data
                    if (result.Success && result.Data != null)
                    {
                        // Assuming result.Data is a single employee object and not a list
                        var employeeData = result.Data as dynamic;

                        if (employeeData != null)
                        {
                            // Create a new EmployeeCredential entity
                            var employeeCredential = new EmployeesCred
                            {
                                EmpId = employeeData[0].EmpId, // Assuming EmpId is part of the employee data
                                Email = employeeData[0].EmailAddress,
                                TempPassword = employeeData[0].Password,
                                FirstName = employeeData[0].FirstName,
                                LastName = employeeData[0].LastName,
                                RoleId = employeeData[0].RoleId,
                                LoginId = employeeData[0].LoginId,
                            };

                            // Save to the EmployeesCred table
                            var employementdetailRepository = _UnitOfWork.GetRepository<EmployeesCred>();
                            await employementdetailRepository.AddAsync(employeeCredential);
                            await _UnitOfWork.SaveAsync();

                            // After saving employee data, update IsProfileCompleted in EmployeeDetails table
                            var employeeDetailsRepository = _UnitOfWork.GetRepository<EmployeeDetail>();

                            // Find the EmployeeDetails record for the given EmpId
                            var employeeDetails = await employeeDetailsRepository.GetEmployeeDetailsByIdAsync(EmpId);

                            if (employeeDetails != null)
                            {
                                // Set IsProfileCompleted to true
                                employeeDetails[0].IsProfileCompleted = true;

                                // Save changes to the EmployeeDetails table
                                await _UnitOfWork.SaveAsync();
                            }
                            else
                            {
                                // Handle the case where EmployeeDetails is not found
                                throw new Exception("EmployeeDetails not found.");
                            }

                            // If everything is successful, commit the transaction
                            transaction.Complete();

                            // Return success response
                            return View("employeeview");
                        }
                        else
                        {
                            return View("employeeview");
                        }
                    }
                    else
                    {
                        // Handle the case where the service failed
                        return View("employeeview");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the error (e.g., log the exception)
                    return Json(new { Success = false, Message = ex.Message });
                }
            }
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
                    Body = "This message is for testing purpose only! Please ignore it, Thanks,"
                };

                // Sending the email
                _emailService.SendEmailAsync(_email.RecipentMail, _email.CcMail, _email.Subject, _email.Body);
                return RedirectToAction("EmployeeProfile"); // Redirect to the list of employee types
            }
            else
            {
                // Error handling for the case where creation fails
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "An error occurred while creating the record.";
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
