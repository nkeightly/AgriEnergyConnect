using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class EditFarmerProfileModel
    {
        public int Id { get; set; }
        public double FarmSize { get; set; } // in acres or hectares
        public string TypeOfFarming { get; set; } // e.g., organic, livestock, crop
        public int YearsOfExperience { get; set; }
        public string Certifications { get; set; }
    }

}
