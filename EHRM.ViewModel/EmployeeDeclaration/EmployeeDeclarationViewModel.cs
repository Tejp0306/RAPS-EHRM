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
        public string? EmployeeName { get; set; }
        public int EmpId { get; set; }
        public string? Designation { get; set; }
        public string? BandLevel { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? ProbationStatus { get; set; }
        public DateTime? ProbationDate { get; set; }
        public string? Location { get; set; }
        public string? Project { get; set; }
        public string? BloodGroup { get; set; }
        public string? DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? SpouseFatherMotherName { get; set; }
        public string? RelationWithSpouse { get; set; }
        public DateTime? SpouseFatherDateOfBirth { get; set; }
        public string? MaritalStatus { get; set; }
        public string? OfficialContact { get; set; }
        public string? PersonalContact { get; set; }
        public string OfficialEmail { get; set; }
        public string? PersonalEmail { get; set; }
        public int TenureInRAPS { get; set; }
        public int YearsInRAPS { get; set; }
        public int PriorWorkExperience { get; set; }
        public int TotalWorkExperience { get; set; }
        public string? FirstOrganisation { get; set; }
        public int FirstOrganisationExperience { get; set; }
        public string? SecondOrganisation { get; set; }
        public int SecondOrganisationExperience { get; set; }
        public string? ThirdOrganisation { get; set; }
        public int ThirdOrganisationExperience { get; set; }
        public string? FourthOrganisation { get; set; }
        public int FourthOrganisationExperience { get; set; }
        public string? Dependent1Name { get; set; }
        public string? Dependent1Relationship { get; set; }
        public DateTime? Dependent1Dob { get; set; }
        public string? EmergencyName1 { get; set; }
        public string? EmergencyContact1 { get; set; }
        public string? EmergencyRelationship1 { get; set; }
        public string? EmergencyName2 { get; set; }
        public string? EmergencyContact2 { get; set; }
        public string? EmergencyRelationship2 { get; set; }
        public string? XthInstitution { get; set; }
        public string? XthPassingYear { get; set; }
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
        public string? PermanentAddress { get; set; }
        public string? PostalAddress { get; set; }
        public string? Ctc { get; set; }
        public string? FilingPerson { get; set; }
        public string? FilingRecheck { get; set; }
        public string? ResignationDate { get; set; }
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
