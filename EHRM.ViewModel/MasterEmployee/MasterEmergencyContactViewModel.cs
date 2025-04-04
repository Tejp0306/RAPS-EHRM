using System.ComponentModel.DataAnnotations;

public class MasterEmergencyContactViewModel
{
    [Key]
    public int EmergencyId { get; set; }

    public int EmpId { get; set; } // Foreign Key

    [Required]
    public string EmergencyName { get; set; }

    [Required]
    public string EmergencyContactNumber { get; set; }

    [Required]
    public string Relationship { get; set; }
}
