using EHRM.ViewModel.Review;
using Microsoft.AspNetCore.Mvc;

namespace EHRM.Web.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProbationEvaluation()
        {
            var questions = new List<EvaluationQuestion>
            {
                new EvaluationQuestion { Id = 1, Text = "Quality of work - The extent to which the employee accomplishes assigned work of a specified quality within a specified time period" },
                new EvaluationQuestion { Id = 2, Text = "Knowledge of Job - The extent to which the employee knows and demonstrates how and why to do all phases of assigned work" },
                new EvaluationQuestion { Id = 3, Text = "Relationship with manager - The manner in which the employee responds to manager directions and comments" },
                new EvaluationQuestion { Id = 4, Text = "Cooperation with others - The extent to which the employee gets along with other individuals" },
                new EvaluationQuestion { Id = 5, Text = "Attendance and Reliability - The extent to which the employee arrives on time and demonstrates consistent attendance" },
                new EvaluationQuestion { Id = 6, Text = "Initiative and creativity - The extent to which the employee is self-directed, resourceful, and creative in meeting job objectives" },
                new EvaluationQuestion { Id = 7, Text = "Capacity to develop - The extent to which the employee demonstrates the ability and willingness to accept new/more complex duties" }
            };

            return View(questions);  // Pass the questions to the view 
        }
        
    }
}
