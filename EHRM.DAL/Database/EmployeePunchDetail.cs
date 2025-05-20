using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployeePunchDetail
{
    public int Id { get; set; }

    public int? Empid { get; set; }

    public string? Month { get; set; }

    public DateOnly? PunchDate { get; set; }

    public string? Punchintime { get; set; }

    public string? Punchouttime { get; set; }

    public decimal? TotalHours { get; set; }

    public string? EmployeeName { get; set; }
}
