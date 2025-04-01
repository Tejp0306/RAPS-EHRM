using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


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
        public DateTime? SignatureDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string? Note { get; set; }
        public decimal? TotalHours { get; set; }
        public List<string> FilePath { get; set; } = new List<string>(); // Fix: Use List<string>
        public IFormFile[]? Files { get; set; }  // Fix: Use an array for multiple files
        public List<DailyEntryModel> DailyEntries { get; set; } = new List<DailyEntryModel>();

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


