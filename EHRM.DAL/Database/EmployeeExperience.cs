using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployeeExperience
{
    public int ExperienceId { get; set; }

    public int EmpId { get; set; }

    public string OrganisationName { get; set; } = null!;

    public decimal YearsOfExperience { get; set; }

    public virtual EmployeeMaster Emp { get; set; } = null!;
}
