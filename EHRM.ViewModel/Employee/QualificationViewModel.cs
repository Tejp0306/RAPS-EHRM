using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Employee
{
    public class QualificationViewModel
    {
        public int Id { get; set; }

        public int? EmpId { get; set; }

        public string? CourseName { get; set; }

        public string? Concentration { get; set; }

        public string? QualificationEarned { get; set; }

        public string? InstitutionName { get; set; }

        public string? CountryName { get; set; }

        public string? PassedDate { get; set; }

        public string? Details { get; set; }

        public string? Document { get; set; }

        public bool? IsActive { get; set; }

        // Additional properties for view purposes
        public string? EmployeeName { get; set; } // Employee Name (if you want to show this in the view)
       
    }

}
