using EHRM.DAL.Database;
using EHRM.ServiceLayer.Employee;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Master;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employee;

        public EmployeeController(IEmployeeService employee)
        {

            _employee = employee;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult addemployee()
        {
            return View();
        }

        public IActionResult employeeview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SavePersonalInfoAsync(EmployeeViewModel model)
        {

            var filepath = "";
            if (model.ProfileImg != null)
            {

                filepath = Upload(model);
            }
            string createdById = "Arjun"; // Replace with logic to fetch the actual user ID
            var result = await _employee.SavePersonalInfoAsync(model, createdById, filepath);
            // Handle the result of the create operation
            if (result.Success)
            {
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Operation completed successfully!";
                return RedirectToAction("employeeview"); // Redirect to the list of roles
            }
            else
            {
                ViewBag.ErrorMessage = result.Message; // Display error message
                return View(model); // Return to the same view with the provided model
            }
        }

      


        // Get Role Data for the role dropdown

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
                        id = role.Id, // Ensure Team class has an Id property
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

        private string Upload(EmployeeViewModel model)
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


        //Get Employee data for employee view data table

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
                        Id=emp.Id,
                        Name = emp.FirstName + ' ' + emp.LastName,
                        Email= emp.EmailAddress


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


        // Get Team Data for the team dropdown

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


    }
    
}
