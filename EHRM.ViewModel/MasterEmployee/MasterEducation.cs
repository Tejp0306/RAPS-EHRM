using System.ComponentModel.DataAnnotations;

public class MasterEducation
{
    [Key]
    public int EducationId { get; set; }

    [Required]
    public int EmpId { get; set; } // Foreign Key linking to EmployeeMaster

    // Xth Details
    [MaxLength(200)]
    public string XthInstitution { get; set; }
    [Range(1900, 2100)]
    public int XthPassingYear { get; set; }

    // XIIth Details
    [MaxLength(200)]
    public string XIIthInstitution { get; set; }
    [Range(1900, 2100)]
    public int XIIthPassingYear { get; set; }

    // Bachelor's Details
    [MaxLength(200)]
    public string BachelorInstitution { get; set; }
    [Range(1900, 2100)]
    public int BachelorCompletionYear { get; set; }
    [MaxLength(100)]
    public string BachelorDegree { get; set; }

    // Master's Details
    [MaxLength(200)]
    public string MasterInstitution { get; set; }
    [Range(1900, 2100)]
    public int MasterCompletionYear { get; set; }
    [MaxLength(100)]
    public string MasterDegree { get; set; }

    // Post-Doctorate Details
    [MaxLength(200)]
    public string PostDoctorateInstitution { get; set; }
    [MaxLength(100)]
    public string PostDoctorateDegree { get; set; }
    [Range(1900, 2100)]
    public int? PostDoctorateCompletionYear { get; set; }

    // Professional Courses
    [MaxLength(200)]
    public string ProfessionalCoursesInstitution { get; set; }
    [MaxLength(100)]
    public string ProfessionalCoursesDegree { get; set; }

    [Range(1900, 2100)]
    public int? ProfessionalCoursesCompletionYear { get; set; }
}
