using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Employee
{
    public class SalaryViewModel
    {
        public int Id { get; set; }

        public int? EmpId { get; set; }

        public double? Ctc { get; set; }

        public string? StartYear { get; set; }

        public string? EndYear { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        // Additional properties for view purposes
        public string? EmployeeName { get; set; } // Employee Name (if you want to show this in the view)

    }

}
