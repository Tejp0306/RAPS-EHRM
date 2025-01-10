using EHRM.ServiceLayer.Self;
using EHRM.ViewModel.Employee;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace EHRM.Web.Controllers
{
    public class SelfController : Controller
    {
        private readonly ISelfService _self;

        public SelfController(ISelfService Self)
        {

            _self = Self;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SelfPortal(int id)
        { 
            var result = await _self.GetAllSelfEmployeeRecordDetails(id);

            if (result.Count > 0)
            {
                TempData["ToastType"] = "success"; // Store success message
                TempData["ToastMessage"] = "User Found";
                var Model = result[0];

                return View(Model);
            }
            else
            {
                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "User may deleted or removed";

                return View();
            }
        }

        public IActionResult Profile(EmployeeViewModel model)
        {
            return View(model);
        }

        public async Task<IActionResult> GetSelfProfileDataByIDDOB(EmployeeViewModel model)
        {
            // Fetch the result from the service layer

            //string Parseddob = DateTime.ParseExact(model.DateOfBirth, "dd - MMMM - yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            var datedata = DateTime.Parse(model.DateOfBirth).ToString("dd/MM/yyyy");


            String updateddatedata = datedata.Substring(0, 10);


            var result = await _self.GetDetailsByEmpIdDOB(model.EmpId, updateddatedata);

            if (result.Count > 0)
            {
                TempData["ToastType"] = "success"; // Store success message
                TempData["ToastMessage"] = "User Found";

                EmployeeViewModel employee = new()
                {
                    EmpId = result[0].EmpId,
                    DateOfBirth = result[0].DateOfBirth
                };

                return View("Profile", employee);
            }
            else
            {
                model.EmpId = 0;

                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "User may deleted or removed";

                return View("GetProfileData", model);
            }
        }
        
    }
}
