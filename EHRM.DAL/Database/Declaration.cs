using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Declaration
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string? HrRepresentativeName { get; set; }

    public string? HrRepresentativeDesignation { get; set; }

    public string? HrContactInfo { get; set; }

    public string? Date { get; set; }

    public string? Signature { get; set; }

    public bool? VerificationCrossCheck { get; set; }

    public bool? VerificationMandatory { get; set; }

    public bool? IsActive { get; set; }

    public virtual EmployeeDetail? Emp { get; set; }
}
