using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class NonDisclosureAgreement
{
    public int Id { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string AgreementDate { get; set; } = null!;

    public string Signature { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
