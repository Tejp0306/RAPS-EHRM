using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class AddressDetail
{
    public int AddressId { get; set; }

    public int? EmpId { get; set; }

    public string? PermanentAddress { get; set; }

    public string? PostalAddress { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
