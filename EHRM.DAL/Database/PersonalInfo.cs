using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class PersonalInfo
{
    public int Id { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string PersonalEmail { get; set; } = null!;

    public string PermanentAddress { get; set; } = null!;

    public string CurrentAddress { get; set; } = null!;

    public string? HomePhone { get; set; }

    public string MobilePhone { get; set; } = null!;

    public string EmergencyContact1Name { get; set; } = null!;

    public string EmergencyContact1Relationship { get; set; } = null!;

    public string EmergencyContact1Phone { get; set; } = null!;

    public string? EmergencyContact2Name { get; set; }

    public string? EmergencyContact2Relationship { get; set; }

    public string? EmergencyContact2Phone { get; set; }

    public string Signature { get; set; } = null!;

    public string FormDate { get; set; } = null!;
}
