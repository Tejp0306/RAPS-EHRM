using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Employee
{
    public class DeclarationViewModel
    {
        public int Id { get; set; }

        public int? EmpId { get; set; }

        public string? HrRepresentativeName { get; set; }

        public string? HrRepresentativeDesignation { get; set; }

        public string? HrContactInfo { get; set; }

        public string? Date { get; set; }

        public string? Signature { get; set; }

        public bool VerificationCrossCheck { get; set; }

        public bool VerificationMandatory { get; set; }

        public bool? IsActive { get; set; }

        // Additional properties for view purposes
        public string? EmployeeName { get; set; } // Employee Name (if you want to show this in the view)
    }

}
