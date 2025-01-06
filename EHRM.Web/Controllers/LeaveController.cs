using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddLeaveType()
        {
            return View();
        }
        public IActionResult LeaveApply()
        {
            return View();
        }
    }
}
