using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class ClientPropertyDeclaration
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string ReceivedDate { get; set; } = null!;

    public string ItemsReceived { get; set; } = null!;

    public string EmployeeNameConfirm { get; set; } = null!;

    public string Signature { get; set; } = null!;

    public string ConfirmationDate { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
