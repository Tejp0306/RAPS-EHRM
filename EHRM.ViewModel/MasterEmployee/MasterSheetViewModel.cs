using System;
using System.ComponentModel.DataAnnotations;

namespace EHRM.ViewModel.MasterEmployee
{
    public class MasterSheetViewModel
    {
        // Existing Employee Information Fields
        public int EmpId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Designation { get; set; }
        public string BandLevel { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string ProbationConfirmationStatus { get; set; }
        public DateTime? ProbationConfirmationDate { get; set; }
        public string Location { get; set; }
        public string FunctionDepartment { get; set; }
        public int? AgeYears { get; set; }
        public string Gender { get; set; }
        public string OfficialContactNo { get; set; }
        public string PersonalContactNo { get; set; }
        public string OfficialEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string MaritalStatus { get; set; }

        // Reporting Details
        public string DirectReporting { get; set; }
        public string DottedReporting { get; set; }
        public string SkipLevelReporting { get; set; }

        // Emergency Contact
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }

        // Dependent Details
        public List<DependentDetail> DependentDetails { get; set; } = new List<DependentDetail>();

        // Education Details
        public List<EducationDetail> EducationDetails { get; set; } = new List<EducationDetail>();

        public class DependentDetail
        {
            public int Id { get; set; }
            public int EmpId { get; set; } // Foreign key to MasterSheet

            // Updated fields to match the view
            public string SpouseFatherMotherName { get; set; } // Changed from DependentName
            public string RelationWithEmployee { get; set; }   // Changed from Relationship
            public DateTime? SpouseFatherDOB { get; set; }     // Changed from DependentDOB

            // Navigation Property (if needed)
            
        }


        public class EducationDetail
        {
            public int Id { get; set; }
            public int EmpId { get; set; } // Foreign key to MasterSheet
            public string InstitutionName { get; set; }
            public string DegreeName { get; set; }
            public int PassingYear { get; set; }
        }



        // Salary & Exit Details
        public string UANNumber { get; set; }
        public string AadharNumber { get; set; }
        public string PanCardNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string PermanentAddress { get; set; }
        public string PostalAddress { get; set; }
        public decimal CTCPerAnnum { get; set; }

        // ✅ New: Add list of Prior Experiences
        public List<PriorExperience> Experiences { get; set; } = new List<PriorExperience>();
    }

    public class PriorExperience
    {
        public int Id { get; set; }
        public string OrganisationName { get; set; }
        public decimal YearsOfExperience { get; set; }
    }

}
