using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class DependentDetail
{
    public int DependentId { get; set; }

    public int? EmpId { get; set; }

    public string? DependentName { get; set; }

    public string? Relationship { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
