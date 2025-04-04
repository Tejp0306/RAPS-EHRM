using System.ComponentModel.DataAnnotations;

public class MasterContactDetails
{
    [Key]
    public int ContactId { get; set; }

    public int EmpId { get; set; } // Foreign Key

    [Phone]
    public string OfficialContactNo { get; set; }

    [Required, Phone]
    public string PersonalContactNo { get; set; }

    [EmailAddress]
    public string OfficialEmailId { get; set; }

    [EmailAddress]
    public string PersonalEmailId { get; set; }
    public string EmergencyContactName { get; set; }
    public string EmergencyContactNumber { get; set; }
    public string EmergencyRelationship { get; set; }
}
