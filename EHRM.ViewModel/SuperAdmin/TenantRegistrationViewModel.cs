using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.SuperAdmin
{
    public class TenantRegistrationViewModel
    {
        public int Id { get; set; } 
        public string? OrganizationID { get; set; }  // Primary key, auto-incremented by database
        public string OrgName { get; set; }  // Organization name
        public string OrgType { get; set; }  // Organization type (e.g., Non-profit, Corporation)
        public int? OrgCapacity { get; set; }  // Number of employees, nullable (in case this is optional)
        public string Date { get; set; }
        public string Address { get; set; }  // Organization's physical address
        public string PostalCode { get; set; }  // Postal code
        public string City { get; set; }  // City where the organization is located
        public string Country { get; set; }  // Country where the organization is located
        public string Email { get; set; }  // Contact email address
        public string Mobile { get; set; }  // Mobile number
        public string PrimaryAdminName { get; set; }  // Primary administrator's name
        public string AdminUsername { get; set; }  // Admin username
        public string PasswordHash { get; set; }  // Password hash (never store plain text passwords)
        public string ConfirmPasswordHash { get; set; }  // Confirm password hash (optional, used only for validation)
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date and time when the record was created
        public DateTime UpdatedAt { get; set; } = DateTime.Now;  // Date and time when the record was last updated

    }
}
