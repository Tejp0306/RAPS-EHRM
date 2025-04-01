using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.ExitFormalities
{
    public class ExitInterviewViewModel
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public string InterviewDate { get; set; }

        public string Interviewer { get; set; }

        public string Strengths { get; set; }

        public string AreasOfImprovement { get; set; }

        public string TreatmentAfterResignation { get; set; }

        public string Rejoin { get; set; }  // "Yes", "No", or "Maybe"

        public string ReasonForLeaving { get; set; }

        public string ComparisonWithNewJob { get; set; }

        public string Recommend { get; set; }

        public string GreatestChallenge { get; set; }

        public string EnjoyedFunctions { get; set; }

        public string LeastEnjoyedFunctions { get; set; }

        public string JobSecurity { get; set; }  // "Yes" or "No"

        public string JobSecurityDetails { get; set; }

        public string DepartmentMorale { get; set; }

        public string ImproveMorale { get; set; }

        public string SupervisorFeedback { get; set; }

        public string WorkingConditions { get; set; }

        public string BenefitsSatisfactory { get; set; }  // "Yes" or "No"

        public string InformedPolicies { get; set; }  // "Yes" or "No"

        public string PoliciesFeedback { get; set; }

        public string ChangeDecision { get; set; }

        public string AdditionalComments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
