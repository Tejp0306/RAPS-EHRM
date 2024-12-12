using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployeesCred
{
    public int Id { get; set; }

    public string? EmpId { get; set; }

    public string Email { get; set; } = null!;

    public string TempPassword { get; set; } = null!;

    public bool Active { get; set; }

    public int FailedLoginAttempts { get; set; }

    public bool IsLockedOut { get; set; }

    public DateTime? LockoutEndTime { get; set; }

    public int? LockoutDuration { get; set; }

    public int? RoleId { get; set; }
}
