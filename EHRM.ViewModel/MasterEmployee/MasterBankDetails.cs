using System.ComponentModel.DataAnnotations;

public class MasterBankDetails
{
    [Key]
    public int BankId { get; set; }

    public int EmpId { get; set; } // Foreign Key

    [Required]
    public string BankName { get; set; }

    [Required]
    public string AccountNumber { get; set; }

    [Required]
    public string IFSCCode { get; set; }
}
