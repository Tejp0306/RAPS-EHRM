using EHRM.ViewModel.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Leave
{
    public class EmployeeLeaveViewModel
    {
        public GetAllEmployeeViewModel Employee { get; set; }
        
        public LeaveSummaryViewModel LeaveSummary { get; set; }
    }
}
