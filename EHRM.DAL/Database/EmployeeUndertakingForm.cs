using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployeeUndertakingForm
{
    public int Id { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string Relation { get; set; } = null!;

    public string FatherName { get; set; } = null!;

    public string PermanentAddress { get; set; } = null!;

    public string OfficeAddress { get; set; } = null!;

    public string LastWorkingDate { get; set; } = null!;

    public string ResignationDate { get; set; } = null!;

    public string? EmployeeSignature { get; set; }

    public DateTime? CreatedAt { get; set; }
}
