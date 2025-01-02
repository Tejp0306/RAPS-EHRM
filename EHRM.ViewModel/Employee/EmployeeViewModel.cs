using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Employee
{
    
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string? PrefixName { get; set; }

        public int EmpId { get; set; }

        public string Title { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string LoginId { get; set; } = null!;

        public string Password { get; set; } = null!;
        
        public IFormFile? ProfileImg { get; set; }
        public string? FileName { get; set; }

        public int RoleId { get; set; }

        public string DateOfBirth { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public int MaritalStatus { get; set; }

        public string AadharNumber { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string? HomePhone { get; set; }

        public string CellPhone { get; set; } = null!;

        public string? OfficePhone { get; set; }

        public int TeamId { get; set; }

        public DateOnly? MarriageAnniversary { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string? ZipCode { get; set; }

        public string Nationality { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
    public class EmployeeName
    {
        public int Id { get; set; }

        public int EmpId { get; set; }

        public string? FullName { get; set; }


    }
}
