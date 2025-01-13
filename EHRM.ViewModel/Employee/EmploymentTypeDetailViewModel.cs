using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Employee
{
    public class EmploymentTypeDetailViewModel
    {
        public int Id { get; set; }

        public int? EmpId { get; set; }

        public int? EmpType { get; set; }

        public string? EmpTypeName { get; set; }

        public string? AppointmentDate { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }

        public double? TotalService { get; set; }

        public double? AppointedService { get; set; }

        public int? EmploymentStatusId { get; set; }

        public int? ManagerId { get; set; }

        public bool? IsActive { get; set; }

        public string? EmployeeName { get; set; } // Additional property for employee name if needed

        public bool EmploymentStatuses { get; set; } // Dropdown for employment statuses
        public int Managers { get; set; } // Dropdown for managers
    }

}
