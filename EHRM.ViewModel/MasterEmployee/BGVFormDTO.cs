using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHRM.ViewModel.MasterEmployee
{
    public class BGVFormDTO
    {
        public int BGVFormId { get; set; }
        public int? EmpId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherFirstName { get; set; }
        public string FatherMiddleName { get; set; }
        public string FatherLastName { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string MobileNumber { get; set; }
        public string AlternateNumber { get; set; }
        public string Email { get; set; }

        // Education Details
        public string CourseName { get; set; }
        public string ProgramType { get; set; }
        public string CollegeName { get; set; }
        public string UniversityBoardName { get; set; }
        public int PassingYear { get; set; }
        public string ProofType { get; set; }

        // Address Details
        public string CompleteAddress { get; set; }
        public string NearestLandmark { get; set; }

        // Reference Details
        public string ReferenceName { get; set; }
        public string ReferenceContact { get; set; }

        // Consent
        public bool ConsentGiven { get; set; }

        // Previous Employment Details
        public List<PreviousEmploymentDTO> PreviousEmployments { get; set; }
    }

    // Previous Employment DTO
    public class PreviousEmploymentDTO
    {
        public int PreviousEmploymentId { get; set; }
        public string NatureOfEmployment { get; set; }
        public string CurrentDesignation { get; set; }
        public string Department { get; set; }
        public string OfficialTitle { get; set; }
        public string PayrollCompanyName { get; set; }
        public string OrganizationAddress { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string EmployeeCode { get; set; }
        public decimal? CTCPerAnnum { get; set; }
        public string KeyResponsibility { get; set; }
        public string EmploymentTenure { get; set; }
        public string ReasonForLeaving { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerDesignation { get; set; }
        public string IsReportingManagerStillInCompany { get; set; }
        public string CompanyLandline { get; set; }
        public string PersonalMobileNo { get; set; }
        public string BestTimeToReach { get; set; }
    }

}
