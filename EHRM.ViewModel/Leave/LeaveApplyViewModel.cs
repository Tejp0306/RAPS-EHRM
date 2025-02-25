using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.ViewModel.Self;

namespace EHRM.ViewModel.Leave
{
    public class LeaveApplyViewModel
    {

        public int Id { get; set; }                  // Unique ID for the leave application
        public int EmpId { get; set; }               // Employee ID (Foreign Key, can reference a specific employee)
        public string? EmployeeName { get; set; }     // Employee's full name
        public string? LeaveType { get; set; }        // Type of leave (Sick Leave, Casual Leave, etc.)
        public string? ApplyDate { get; set; }      // Date when the leave was applied
        public string? LeaveFrom { get; set; }      // Start date of the leave
        public string? LeaveTo { get; set; }        // End date of the leave
        public int TotalDays { get; set; }           // Total number of days the leave spans

        public string? Description { get; set; }      // Reason for the leave
        public DateTime CreatedAt { get; set; }      // Timestamp when the leave application was created
        public DateTime UpdatedAt { get; set; }      // Timestamp for when the leave application was last updated

        public LeaveStatusViewModel Status { get; set; }

        //public LeaveApplyViewModel()
        //{
        //    Status = new LeaveStatusViewModel();
        //}


    }

    public class LeaveStatusViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string? LeaveStatus { get; set; }      // Status of the leave (Pending, Approved, etc.) 
        public string? ManagerRemark { get; set; }    // Remarks by the Manager (nullable)
    }
}
