using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class ResignationForm
{
    public int Id { get; set; }

    public string EmployeeSignature { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string FinalDay { get; set; } = null!;

    public string ResignationDate { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string TotalMonths { get; set; } = null!;
}
