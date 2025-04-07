using System.ComponentModel.DataAnnotations;

public class MasterReportingDetails
{
    [Key]
    public int ReportingId { get; set; }

    [Required]
    public int EmpId { get; set; } // Foreign Key

    [StringLength(100)]
    public string DirectReporting { get; set; }

    [StringLength(100)]
    public string DottedReporting { get; set; }

    [StringLength(100)]
    public string SkipReporting { get; set; }
}
