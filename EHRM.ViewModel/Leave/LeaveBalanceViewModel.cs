using EHRM.ViewModel.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Leave;

public class LeaveBalanceViewModel
{
    public int EmpId { get; set; }
    public int TenureYears { get; set; }
    public int EarnedLeave { get; set; }
    public int SickLeave { get; set; }
    public int CasualLeave { get; set; }
    public int TotalLeave { get; set; }
}

