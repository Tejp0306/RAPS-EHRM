using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class DailyEntry
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public int? TimeSheetId { get; set; }

    public DateOnly? DayDate { get; set; }

    public string? DayOfWeek { get; set; }

    public decimal? HoursWorked { get; set; }

    public string? AssignmentDesc { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TimeSheet? TimeSheet { get; set; }
}
