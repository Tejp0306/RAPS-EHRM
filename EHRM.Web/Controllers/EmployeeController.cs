using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

    }
}
