using System;
using System.ComponentModel.DataAnnotations;

public class MasterFamilyDetails
{
    [Key]
    public int FamilyId { get; set; }

    [Required]
    public int EmpId { get; set; } // Foreign Key

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(50)]
    public string RelationWithEmployee { get; set; }


    //public DateOnly DateOfBirth { get; set; }
    public DateTime DateOfBirth { get; set; }
}
