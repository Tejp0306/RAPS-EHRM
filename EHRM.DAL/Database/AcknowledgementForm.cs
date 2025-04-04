using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class AcknowledgementForm
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string? EmployeeSignature { get; set; }

    public string SignatureDate { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
