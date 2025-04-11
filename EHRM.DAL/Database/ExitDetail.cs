using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class ExitDetail
{
    public int ExitId { get; set; }

    public int? EmpId { get; set; }

    public DateOnly? ResignationDate { get; set; }

    public DateOnly? TerminationDate { get; set; }

    public DateOnly? DateOfExit { get; set; }

    public string? ReasonForLeaving { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
