using System;
using System.ComponentModel.DataAnnotations;

public class MasterDependentDetails
{
    [Key]
    public int DependentId { get; set; }

    [Required]
    public int EmpId { get; set; } // Foreign Key

    [Required, StringLength(100)]
    public string DependentName { get; set; }

    [Required, StringLength(50)]
    public string Relationship { get; set; }

 
    //public DateOnly DateOfBirth { get; set; }

    public DateTime DateOfBirth { get; set; }
}
