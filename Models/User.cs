using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Models
{
    public class User : IdentityUser<int>
    {
        public string? Role { get; set; }
    }
}
