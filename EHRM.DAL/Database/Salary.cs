using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Salary
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public double? Ctc { get; set; }

    public string? StartYear { get; set; }

    public string? Endyear { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual EmployeeDetail? Emp { get; set; }
}
