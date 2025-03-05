using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class LeavePolicy
{
    public int PolicyId { get; set; }

    public int YearsOfServiceMin { get; set; }

    public int YearsOfServiceMax { get; set; }

    public int CasualLeave { get; set; }

    public int SickLeave { get; set; }

    public int EarnedLeave { get; set; }

    public decimal EarnedLeaveAccrualRate { get; set; }

    public int CarryForwardLimit { get; set; }

    public bool IsEarnedLeaveCarriedForward { get; set; }
}
