using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class ContactDetail
{
    public int ContactId { get; set; }

    public int? EmpId { get; set; }

    public string? OfficialContactNo { get; set; }

    public string? PersonalContactNo { get; set; }

    public string? OfficialEmailId { get; set; }

    public string? PersonalEmailId { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactNumber { get; set; }

    public string? EmergencyRelationship { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
