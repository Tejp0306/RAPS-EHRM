using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class LeaveApply
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string LeaveType { get; set; } = null!;

    public string ApplyDate { get; set; } = null!;

    public string LeaveFrom { get; set; } = null!;

    public string LeaveTo { get; set; } = null!;

    public int? TotalDays { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
