using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Manager
{
    public int Id { get; set; }

    public int? ManagerId { get; set; }

    public string? Name { get; set; }

    public string? Discription { get; set; }
}
