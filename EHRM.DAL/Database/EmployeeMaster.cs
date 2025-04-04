using System;
using System.Collections.Generic;

namespace EHRM.DAL.Database;

public partial class EmployeeMaster
{
    public int SrNo { get; set; }

    public int EmpId { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? Age { get; set; }

    public string? MaritalStatus { get; set; }

    public DateOnly? DateOfJoining { get; set; }

    public string? BandLevel { get; set; }

    public string? Designation { get; set; }

    public string? Location { get; set; }

    public string? Department { get; set; }

    public string? FunctionProject { get; set; }

    public string? ProbationConfirmationStatus { get; set; }

    public DateOnly? ProbationConfirmationDate { get; set; }

    public decimal? TenureInRaps { get; set; }

    public decimal? YearsInRaps { get; set; }

    public decimal? TotalWorkExperience { get; set; }

    public string? Uannumber { get; set; }

    public string? AadharNumber { get; set; }

    public string? PancardNumber { get; set; }

    public decimal? CtcperAnnumOnDoj { get; set; }

    public string? FilingPerson { get; set; }

    public string? FilingRecheck { get; set; }

    public string? Remark { get; set; }

    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<BankDetail> BankDetails { get; set; } = new List<BankDetail>();

    public virtual ICollection<ContactDetail> ContactDetails { get; set; } = new List<ContactDetail>();

    public virtual ICollection<DependentDetail> DependentDetails { get; set; } = new List<DependentDetail>();

    public virtual ICollection<EducationalDetail> EducationalDetails { get; set; } = new List<EducationalDetail>();

    public virtual ICollection<ExitDetail> ExitDetails { get; set; } = new List<ExitDetail>();

    public virtual ICollection<FamilyDetail> FamilyDetails { get; set; } = new List<FamilyDetail>();

    public virtual ICollection<MasterEmergencyContact> MasterEmergencyContacts { get; set; } = new List<MasterEmergencyContact>();

    public virtual ICollection<MasterPreviousEmployment> MasterPreviousEmployments { get; set; } = new List<MasterPreviousEmployment>();

    public virtual ICollection<ReportingDetail> ReportingDetails { get; set; } = new List<ReportingDetail>();

    public virtual ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
}
