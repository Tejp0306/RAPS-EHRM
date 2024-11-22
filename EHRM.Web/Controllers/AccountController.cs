using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
