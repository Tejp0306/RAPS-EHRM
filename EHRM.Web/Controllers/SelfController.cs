using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class SelfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelfPortal()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult TimeSheet()
        {
            return View();
        }
      
    }
}
