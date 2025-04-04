using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class FamilyDetail
{
    public int FamilyId { get; set; }

    public int? EmpId { get; set; }

    public string? Name { get; set; }

    public string? RelationWithEmployee { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
