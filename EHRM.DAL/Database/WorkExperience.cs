﻿using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class WorkExperience
{
    public int EmploymentId { get; set; }

    public int EmpId { get; set; }

    public string OrganisationName { get; set; } = null!;

    public string? Designation { get; set; }

    public decimal? YearsOfExperience { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? ReasonForLeaving { get; set; }

    public virtual EmployeeMaster Emp { get; set; } = null!;
}
