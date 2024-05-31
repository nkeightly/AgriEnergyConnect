using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class RegisterViewModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        public required string Role { get; set; }

        // Only applicable for farmers
        public string? Location { get; set; }
        public string? ContactInfo { get; set; }

        public string? ImagePath { get; set; }
        public IFormFile? Image { get; set; }

        // Only applicable for employees
        public string? Department { get; set; }
        public string? Position { get; set; }
    }
}
