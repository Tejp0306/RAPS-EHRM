using System;
using System.ComponentModel.DataAnnotations;

//public class MasterEmployee
//{
//    [Key]
//    public int EmpId { get; set; }

//    [StringLength(100)]
//    public string FirstName { get; set; }

//    [StringLength(100)]
//    public string MiddleName { get; set; }

//    [Required, StringLength(100)]
//    public string LastName { get; set; }

//    [Required]
//    public string Gender { get; set; }


//    public DateOnly DateOfBirth { get; set; }


//    public int Age { get; set; }

//    [StringLength(20)]
//    public string MaritalStatus { get; set; }


//    public DateOnly DateOfJoining { get; set; }

//    [StringLength(50)]
//    public string BandLevel { get; set; }

//    [StringLength(100)]
//    public string Designation { get; set; }

//    [StringLength(100)]
//    public string Location { get; set; }

//    [StringLength(100)]
//    public string Department { get; set; }

//    [StringLength(100)]
//    public string FunctionProject { get; set; }

//    [StringLength(50)]
//    public string ProbationConfirmationStatus { get; set; }


//    public DateOnly ProbationConfirmationDate { get; set; }  // Nullable DateOnly


//    public decimal TenureInRAPS { get; set; }

//    public int YearsInRAPS { get; set; }

//    public decimal TotalWorkExperience { get; set; }

//    [StringLength(20)]
//    public string UANNumber { get; set; }

//    [StringLength(12)]
//    public string AadharNumber { get; set; }

//    [StringLength(10)]
//    public string PANCardNumber { get; set; }


//    public decimal CTCPerAnnumOnDOJ { get; set; }

//    [StringLength(100)]
//    public string FilingPerson { get; set; }

//    [StringLength(100)]
//    public string FilingRecheck { get; set; }

//    [StringLength(500)]
//    public string Remark { get; set; }
//}




public class MasterEmployee
{
    [Key]
    public int EmpId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; }

    [StringLength(100)]
    public string MiddleName { get; set; }

    [Required, StringLength(100)]
    public string LastName { get; set; }

    [Required]
    public string Gender { get; set; }


    public DateTime DateOfBirth { get; set; }


    public int Age { get; set; }

    [StringLength(20)]
    public string MaritalStatus { get; set; }


    // public DateOnly DateOfJoining { get; set; }
    public DateTime DateOfJoining { get; set; }

    [StringLength(50)]
    public string BandLevel { get; set; }

    [StringLength(100)]
    public string Designation { get; set; }

    [StringLength(100)]
    public string Location { get; set; }

    [StringLength(100)]
    public string Department { get; set; }

    [StringLength(100)]
    public string FunctionProject { get; set; }

    [StringLength(50)]
    public string ProbationConfirmationStatus { get; set; }


    //public DateOnly ProbationConfirmationDate { get; set; }  // Nullable DateOnly
    public DateTime ProbationConfirmationDate { get; set; }  // Nullable DateOnly


    public decimal TenureInRAPS { get; set; }

    public int YearsInRAPS { get; set; }

    public decimal TotalWorkExperience { get; set; }

    [StringLength(20)]
    public string UANNumber { get; set; }

    [StringLength(12)]
    public string AadharNumber { get; set; }

    [StringLength(10)]
    public string PANCardNumber { get; set; }


    public decimal CTCPerAnnumOnDOJ { get; set; }

    [StringLength(100)]
    public string FilingPerson { get; set; }

    [StringLength(100)]
    public string FilingRecheck { get; set; }

    [StringLength(500)]
    public string Remark { get; set; }
   // public List<WorkExperienceJson> WorkExperience { get; set; }
}


//public class WorkExperienceJson
//{
//    public string OrganisationName { get; set; }
//    public string Designation { get; set; }
//    public string ReasonForLeaving { get; set; }
//    public DateTime WorkStartDate { get; set; }
//    public DateTime WorkEndDate { get; set; }
//    public double YearsOfExperience { get; set; }
//}

