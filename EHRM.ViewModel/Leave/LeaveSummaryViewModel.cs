using EHRM.ViewModel.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Leave;

public class LeaveSummaryViewModel

{
    public int CasualLeave { get; set; }               // Number of casual leave days
    public int SickLeave { get; set; }                 // Number of sick leave days
    public int EarnedLeave { get; set; }               // Number of earned leave days
    public decimal EarnedLeaveAccrualRate { get; set; }  // Earned leave accrual rate per month
    public int CarryForwardLimit { get; set; }         // Maximum days that can be carried forward
    public decimal Tenure { get; set; }         // Maximum days that can be carried forward

    public bool CarryForwardCL {  get; set; }
    public bool CarryForwardSL { get; set; }
    public bool CarryForwardEL { get; set; }
    public int TotalLeave { get; set; }

}
