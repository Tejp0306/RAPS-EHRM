using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Qualification
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string? CourseName { get; set; }

    public string? Concentration { get; set; }

    public string? QualificationEarned { get; set; }

    public string? InstitutionName { get; set; }

    public string? CountryName { get; set; }

    public string? PassedDate { get; set; }

    public string? Details { get; set; }

    public string? Document { get; set; }

    public bool? IsActive { get; set; }

    public virtual EmployeeDetail? Emp { get; set; }
}
