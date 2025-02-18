using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class TimeSheet
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

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

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DailyEntry> DailyEntries { get; set; } = new List<DailyEntry>();
}
