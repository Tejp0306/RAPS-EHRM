using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class Bgvform
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string FatherFirstName { get; set; } = null!;

    public string? FatherMiddleName { get; set; }

    public string FatherLastName { get; set; } = null!;

    public string DateOfBirth { get; set; } = null!;

    public string PlaceOfBirth { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string MaritalStatus { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public string? AlternateNumber { get; set; }

    public string Email { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public string ProgramType { get; set; } = null!;

    public string CollegeName { get; set; } = null!;

    public string UniversityBoardName { get; set; } = null!;

    public int PassingYear { get; set; }

    public string ProofType { get; set; } = null!;

    public string CompleteAddress { get; set; } = null!;

    public string? NearestLandmark { get; set; }

    public string ReferenceName { get; set; } = null!;

    public string ReferenceContact { get; set; } = null!;

    public bool ConsentGiven { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PreviousEmployment> PreviousEmployments { get; set; } = new List<PreviousEmployment>();
}
