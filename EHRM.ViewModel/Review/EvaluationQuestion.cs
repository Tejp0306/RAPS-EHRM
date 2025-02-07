using System.ComponentModel.DataAnnotations;
using EHRM.ViewModel.Employee;

namespace EHRM.ViewModel.Review
{
    public class EvaluationQuestion
    {
        public EvaluationQuestion()
        {
            Items = new List<EvaluationQuestion>();
        }

        public int Id { get; set; }
        public string Question { get; set; } = null!;

        public int EmpId { get; set; }                    // Employee ID (Foreign Key)

        public string ApplicationDate { get; set; }       // Application Date (e.g., probation start date)

        public int ManagerId { get; set; }                 // Manager ID (Foreign Key)

        public int? QuestionId { get; set; }              // Question ID (nullable in case it's not always required)

        public int? Evaluation1stMonth { get; set; }      // Evaluation marks for the 1st month

        public int? Evaluation2ndMonth { get; set; }      // Evaluation marks for the 2nd month

        public int? Evaluation3rdMonth { get; set; }

        public string? Recommendation { get; set; }        // Manager's recommendation (text field)

        public string? RemarksConfirmation { get; set; }   // Remarks and confirmation from the manager

        public string? ManagerSignature { get; set; }      // Manager's signature (text)

        public string? FinalDate { get; set; }             // Final evaluation date (date when the process is completed)

        public DateTime CreatedAt { get; set; }           // Tracks when the record was created
        public DateTime UpdatedAt { get; set; }

        public bool? IsActive { get; set; }

        public IList<EvaluationQuestion> Items { get; set; }

        public IList<Ques> Ques { get; set; }

        public EmployeeViewModel Details { get; set; }
    }

    public class Ques()
    {
        public int QuestionId { get; set; }              

        public int Evaluation1stMonth { get; set; }      

        public int Evaluation2ndMonth { get; set; }      

        public int Evaluation3rdMonth { get; set; }
    }
}
