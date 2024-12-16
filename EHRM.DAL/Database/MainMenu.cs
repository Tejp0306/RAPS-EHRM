using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class MainMenu
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Icon { get; set; }

    public bool? IsActive { get; set; }
}
