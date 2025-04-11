using System.ComponentModel.DataAnnotations;

public class MasterAddress
{
    [Key]
    public int AddressId { get; set; }

    public int EmpId { get; set; } // Foreign Key

    [Required]
    public string PermanentAddress { get; set; }

    public string PostalAddress { get; set; }
}
