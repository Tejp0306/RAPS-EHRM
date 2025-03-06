using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class LeaveBalance
{
    public int Id { get; set; }

    public int EmpId { get; set; }

    public int TenureYears { get; set; }

    public int EarnedLeave { get; set; }

    public int SickLeave { get; set; }

    public int CasualLeave { get; set; }

    public int? TotalLeave { get; set; }

    public virtual EmployeeDetail Emp { get; set; } = null!;
}
