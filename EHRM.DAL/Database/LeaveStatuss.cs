using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class LeaveStatuss
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string? LeaveStatus { get; set; }

    public string ManagerRemark { get; set; } = null!;

    public bool? IsActive { get; set; }
}
