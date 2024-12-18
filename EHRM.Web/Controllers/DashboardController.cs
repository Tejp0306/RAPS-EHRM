using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
       
    }
}
