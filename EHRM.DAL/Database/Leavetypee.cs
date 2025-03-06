using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Leavetypee
{
    public int Id { get; set; }

    public string LeaveType { get; set; } = null!;

    public string LeaveDescription { get; set; } = null!;

    public bool? IsActive { get; set; }
}
