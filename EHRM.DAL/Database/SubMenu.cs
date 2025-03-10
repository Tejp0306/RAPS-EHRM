using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class SubMenu
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public int? MainMenuId { get; set; }

    public int? RoleId { get; set; }

    public int? EmpId { get; set; }

    public bool? IsActive { get; set; }

    public virtual EmployeeDetail? Emp { get; set; }

    public virtual MainMenu? MainMenu { get; set; }
}
