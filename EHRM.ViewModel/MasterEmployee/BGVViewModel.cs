using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHRM.ViewModel.MasterEmployee
{
    public class BGVViewModel
    {

        public int EmpId { get; set; }
        // ✅ Personal Details
        [Required]
        public string FirstName { get; set; }
        

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FatherFirstName { get; set; }

        public string FatherMiddleName { get; set; }

        [Required]
        public string FatherLastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }

        [Required]
        public string PlaceOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string MaritalStatus { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        [Phone]
        public string AlternateNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // ✅ Education Details
        [Required]
        public string CourseName { get; set; }

        [Required]
        public string ProgramType { get; set; } // Full Time / Part Time

        [Required]
        public string CollegeName { get; set; }

        [Required]
        public string UniversityBoardName { get; set; }

        [Required]
        [Range(1900, 2100, ErrorMessage = "Enter a valid year between 1900 and 2100")]
        public int PassingYear { get; set; }

        [Required]
        public string ProofType { get; set; } // All Semester Marksheet / Final Year Marksheet

        // ✅ Current Address Details
        [Required]
        public string CompleteAddress { get; set; }

        public string NearestLandmark { get; set; }

        // ✅ Reference Details
        [Required]
        public string ReferenceName { get; set; }

        [Required]
        [Phone]
        public string ReferenceContact { get; set; }

        // ✅ Authorization and Consent
        [Required]
        public bool ConsentGiven { get; set; }

        // ✅ List for Previous Employment Details
        public List<PreviousEmploymentDetail> PreviousEmployments { get; set; }
    }

    // 🎯 New Model for Previous Employment Details
    public class PreviousEmploymentDetail
    {
        [Required]
        public string NatureOfEmployment { get; set; }

        [Required]
        public string CurrentDesignation { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string PayrollCompanyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Required]
        [Range(10000, 10000000, ErrorMessage = "CTC should be between 10,000 and 10,000,000.")]
        public decimal CTCPerAnnum { get; set; }

        [Required]
        public string ReasonForLeaving { get; set; }
    }
}
