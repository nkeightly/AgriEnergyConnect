using AgriEnergyConnect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EmployeeProfile
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Department { get; set; }
    [Required]
    public string Position { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}
