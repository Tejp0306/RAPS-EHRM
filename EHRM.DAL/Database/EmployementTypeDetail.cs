using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployementTypeDetail
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public int? EmpType { get; set; }

    public string? AppointmentDate { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public double? TotalService { get; set; }

    public double? AppointedService { get; set; }

    public int? EmploymentStatusId { get; set; }

    public int? ManagerId { get; set; }

    public bool? IsActive { get; set; }

    public virtual EmployeeDetail? Emp { get; set; }
}
