using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EducationalDetail
{
    public int EducationId { get; set; }

    public int? EmpId { get; set; }

    public string? XthInstitution { get; set; }

    public int? XthPassingYear { get; set; }

    public string? XiithInstitution { get; set; }

    public int? XiithPassingYear { get; set; }

    public string? BachelorInstitution { get; set; }

    public string? BachelorDegree { get; set; }

    public int? BachelorCompletionYear { get; set; }

    public string? MasterInstitution { get; set; }

    public string? MasterDegree { get; set; }

    public int? MasterCompletionYear { get; set; }

    public string? PostDoctorateInstitution { get; set; }

    public string? PostDoctorateDegree { get; set; }

    public int? PostDoctorateCompletionYear { get; set; }

    public string? ProfessionalCoursesInstitution { get; set; }

    public string? ProfessionalCoursesDegree { get; set; }

    public int? ProfessionalCoursesCompletionYear { get; set; }

    public virtual EmployeeMaster? Emp { get; set; }
}
