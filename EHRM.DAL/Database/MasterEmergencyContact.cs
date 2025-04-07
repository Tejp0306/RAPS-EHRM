using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class MasterEmergencyContact
{
    public int EmergencyId { get; set; }

    public int EmpId { get; set; }

    public string EmergencyName { get; set; } = null!;

    public string EmergencyContactNumber { get; set; } = null!;

    public string Relationship { get; set; } = null!;

    public virtual EmployeeMaster Emp { get; set; } = null!;
}
