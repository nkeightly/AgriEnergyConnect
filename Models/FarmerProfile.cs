using AgriEnergyConnect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class FarmerProfile
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string ContactInfo { get; set; }
    public string ImagePath { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }

    // Navigation property to represent the relationship with Product
    public ICollection<Product> Products { get; set; }

    // Additional details
    public double FarmSize { get; set; } // in acres or hectares

    public string TypeOfFarming { get; set; } // e.g., organic, livestock, crop

    public int YearsOfExperience { get; set; }

    public string Certifications { get; set; }

}
