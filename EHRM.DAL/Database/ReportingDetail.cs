using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class ReportingDetail
{
    public int ReportingId { get; set; }

    public int? EmpId { get; set; }

    public string? DirectReporting { get; set; }

    public string? DottedReporting { get; set; }

    public string? SkipReporting { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
