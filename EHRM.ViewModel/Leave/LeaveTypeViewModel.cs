using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Leave
{
    public class LeaveTypeViewModel
    {
        public int Id { get; set; }
        public string? LeaveType { get; set; }
        public string? LeaveDescription { get; set; }
        public bool IsActive { get; set; }
    }

}
