using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmpType
{
    public int Id { get; set; }

    public string? EmpType1 { get; set; }

    public bool? IsActive { get; set; }
}
