using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployeesCred
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string Email { get; set; } = null!;

    public string TempPassword { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? RoleId { get; set; }

    public bool Active { get; set; }

    public int FailedLoginAttempts { get; set; }

    public bool IsLockedOut { get; set; }

    public DateTime? LockoutEndTime { get; set; }

    public int? LockoutDuration { get; set; }

    public string? LoginId { get; set; }

    public virtual EmployeeDetail? Emp { get; set; }
}
