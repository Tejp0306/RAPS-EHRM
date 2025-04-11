using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class BankDetail
{
    public int BankId { get; set; }

    public int? EmpId { get; set; }

    public string? BankName { get; set; }

    public string? AccountNumber { get; set; }

    public string? Ifsccode { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
