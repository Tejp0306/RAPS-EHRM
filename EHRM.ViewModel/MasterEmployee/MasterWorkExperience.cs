using System;
using System.ComponentModel.DataAnnotations;

public class MasterWorkExperience
{
    [Key]
    public int WorkExperienceId { get; set; }

    [Required]
    public int EmpId { get; set; } // Foreign Key

    [Required, StringLength(255)]
    public string OrganisationName { get; set; }

    [StringLength(100)]
    public string Designation { get; set; }

    [Required, Range(0, 100)]
    public decimal YearsOfExperience { get; set; } // Changed to decimal for accuracy


    public DateOnly FromDate { get; set; }


    public DateOnly ToDate { get; set; }

    [StringLength(255)]
    public string ReasonForLeaving { get; set; }
}
