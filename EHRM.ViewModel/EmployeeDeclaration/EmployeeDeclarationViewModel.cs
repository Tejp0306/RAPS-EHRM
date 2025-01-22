using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.EmployeeDeclaration
{
    public class EmployeeDeclarationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee Name is required.")]
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        public int EmpId { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string? Designation { get; set; }

        [Required(ErrorMessage = "Band Level is required.")]
        public string? BandLevel { get; set; }

        [Required(ErrorMessage = "Date of Joining is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfJoining { get; set; }

        [Required(ErrorMessage = "Probation Status is required.")]
        public string? ProbationStatus { get; set; }

        [Required(ErrorMessage = "Probation Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ProbationDate { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Project is required.")]
        public string? Project { get; set; }

        [Required(ErrorMessage = "Blood Group is required.")]
        public string? BloodGroup { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? DateOfBirth { get; set; }

        [Range(18, 100, ErrorMessage = "Age should be between 18 and 100 years.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Spouse/Father/Mother Name is required.")]
        public string? SpouseFatherMotherName { get; set; }

        [Required(ErrorMessage = "Relation with Spouse is required.")]
        public string? RelationWithSpouse { get; set; }

        [Required(ErrorMessage = "Spouse/Father Date of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SpouseFatherDateOfBirth { get; set; }

        [Required(ErrorMessage = "Marital Status is required.")]
        public string? MaritalStatus { get; set; }

        [Phone(ErrorMessage = "Invalid Official Contact Number.")]
        public string? OfficialContact { get; set; }

        [Phone(ErrorMessage = "Invalid Personal Contact Number.")]
        public string? PersonalContact { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Official Email Address.")]
        public string OfficialEmail { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Personal Email Address.")]
        public string? PersonalEmail { get; set; }

        public int TenureInRAPS { get; set; }
        public int YearsInRAPS { get; set; }
        public int PriorWorkExperience { get; set; }
        public int TotalWorkExperience { get; set; }

        [Required(ErrorMessage = "First Organisation is required.")]
        public string? FirstOrganisation { get; set; }

        public int FirstOrganisationExperience { get; set; }

        [Required(ErrorMessage = "Second Organisation is required.")]
        public string? SecondOrganisation { get; set; }

        public int SecondOrganisationExperience { get; set; }

        [Required(ErrorMessage = "Third Organisation is required.")]
        public string? ThirdOrganisation { get; set; }

        public int ThirdOrganisationExperience { get; set; }

        [Required(ErrorMessage = "Fourth Organisation is required.")]
        public string? FourthOrganisation { get; set; }

        public int FourthOrganisationExperience { get; set; }

        [Required(ErrorMessage = "Dependent 1 Name is required.")]
        public string? Dependent1Name { get; set; }

        [Required(ErrorMessage = "Dependent 1 Relationship is required.")]
        public string? Dependent1Relationship { get; set; }

        [Required(ErrorMessage = "Dependent 1 Date of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Dependent1Dob { get; set; }

        public string? EmergencyName1 { get; set; }
        public string? EmergencyContact1 { get; set; }
        public string? EmergencyRelationship1 { get; set; }
        public string? EmergencyName2 { get; set; }
        public string? EmergencyContact2 { get; set; }
        public string? EmergencyRelationship2 { get; set; }

        [Required(ErrorMessage = "Xth Institution is required.")]
        public string? XthInstitution { get; set; }

        public string? XthPassingYear { get; set; }

        [Required(ErrorMessage = "Xiith Institution is required.")]
        public string? XiithInstitution { get; set; }

        public string? XiithPassingYear { get; set; }

        public string? BachelorInstitution { get; set; }

        public string? BachelorCompleteYear { get; set; }

        public string? BachelorDegrees { get; set; }

        public string? MasterInstitution { get; set; }

        public string? MasterCompleteYear { get; set; }

        public string? FatherHusbandName { get; set; }
        public string? UanNo { get; set; }
        public string? AdharNo { get; set; }
        public string? PanCardNo { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IfscCode { get; set; }

        [Required(ErrorMessage = "Permanent Address is required.")]
        public string? PermanentAddress { get; set; }

        [Required(ErrorMessage = "Postal Address is required.")]
        public string? PostalAddress { get; set; }

        public string? Ctc { get; set; }

        public string? FilingPerson { get; set; }
        public string? FilingRecheck { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? ResignationDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? ExitDate { get; set; }

        public string? ReasonForLeaving { get; set; }
    }

    public class EmailServiceModel
    {
        public string RecipentMail { get; set; }
        public string CcMail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string FirstName { get; set; }

        public string Password { get; set; }


    }
}
