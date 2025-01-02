using EHRM.Infrastructure.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{

    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult Dashboard()
        
        {
            var userSession = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(userSession))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
       
    }
}
