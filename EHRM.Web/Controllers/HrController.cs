using EHRM.ServiceLayer.HR;
using EHRM.ViewModel.Employee;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class HrController : Controller
    {
        private readonly IHrService _hr;

        public HrController(IHrService Hr)
        {

            _hr = Hr;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async  Task<IActionResult> VerificationScreen(int id)
        {
            // Fetch the result from the service layer
            var result = await _hr.GetAllEmployeeRecordDetails(id);

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
            //return View();
        }
        


        public IActionResult GetProfileData(EmployeeViewModel model)
            {
            return View(model);
        }



        public async Task<IActionResult> GetProfileDataByIDDOB(EmployeeViewModel model)
        {
            // Fetch the result from the service layer

            //string Parseddob = DateTime.ParseExact(model.DateOfBirth, "dd - MMMM - yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            var datedata = DateTime.Parse(model.DateOfBirth).ToString("dd/MM/yyyy");


            String updateddatedata =datedata.Substring(0, 10);
            

            var result = await _hr.GetDetailsByEmpIdDOB(model.EmpId, updateddatedata);

            if (result.Count > 0)
            {
                TempData["ToastType"] = "success"; // Store success message
                TempData["ToastMessage"] = "User Found";

                EmployeeViewModel employee = new()
                {
                    EmpId = result[0].EmpId,
                    DateOfBirth = result[0].DateOfBirth
                };

                return View("GetProfileData", employee);
            }
            else
            {
                model.EmpId = 0;

                TempData["ToastType"] = "danger"; // Store error message
                TempData["ToastMessage"] = "User may deleted or removed";

                return View("GetProfileData",model);
            }
        }



    }
}
