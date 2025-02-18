using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EHRM.ViewModel.Self
{
    public class TimeSheetViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }

        public string? PresentMonth { get; set; }

        public string? EmpName { get; set; }

        public string? ClientName { get; set; }

        public string? Position { get; set; }

        public string? ProjectName { get; set; }

        public string? EmployeeSignature { get; set; }

        public string? ManagerSignature { get; set; }

        public DateTime? SignatureDate { get; set; } // Nullable DateTime

        public DateTime? SubmissionDate { get; set; } // Nullable DateTime

        public string? Note { get; set; }

        public decimal? TotalHours { get; set; }

        //public DateTime? CreatedAt { get; set; }

        //public DateTime? UpdatedAt { get; set; }

        // Collection of DailyEntries for this TimeSheet
        public List<DailyEntryModel> DailyEntries { get; set; }

        public TimeSheetViewModel()
        {
            // Initializing DailyEntries to avoid null reference errors
            DailyEntries = new List<DailyEntryModel>();
        }        
    }
    public class DailyEntryModel
    {
        public string DayDate { get; set; }
        public string DayOfWeek { get; set; }
        public string HoursWorked { get; set; }
        public string AssignmentDesc { get; set; }
        public string Remarks { get; set; }
    }
}


