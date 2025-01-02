using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class HrController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerificationScreen()
        {
            return View();
        }
    }
}
