using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;

        public EmployeeController(ApplicationDbContext context, SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public IActionResult Dashboard()
        {
            var farmerProfiles = _context.FarmerProfiles.ToList();
            return View(farmerProfiles);
        }


        public IActionResult AddFarmerProfile()
        {
            var incompleteProfiles = _context.FarmerProfiles.Where(fp => string.IsNullOrEmpty(fp.TypeOfFarming)).ToList();
            Console.WriteLine($"Number of incomplete profiles: {incompleteProfiles.Count}");
            return View(incompleteProfiles.AsEnumerable());
        }


        public IActionResult GetFarmerDetails(int farmerId)
        {
            var farmer = _context.FarmerProfiles.FirstOrDefault(fp => fp.Id == farmerId);
            if (farmer == null)
            {
                return NotFound();
            }
            var farmerDetails = new
            {
                farmer.Name,
                farmer.FarmSize,
                farmer.TypeOfFarming,
                farmer.YearsOfExperience,
                farmer.Certifications
            };
            return Json(farmerDetails);
        }
        public IActionResult EditFarmerProfile(int id)
        {
            var farmerProfile = _context.FarmerProfiles.Find(id);
            if (farmerProfile == null)
            {
                return NotFound();
            }
            return View(farmerProfile);
        }
        [HttpPost]
        public async Task<IActionResult> EditFarmerProfile(EditFarmerProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var existingFarmer = await _context.FarmerProfiles.FindAsync(model.Id);
                if (existingFarmer == null)
                {
                    return NotFound();
                }

                // Update only the fields that were changed
                existingFarmer.FarmSize = model.FarmSize;
                existingFarmer.TypeOfFarming = model.TypeOfFarming;
                existingFarmer.YearsOfExperience = model.YearsOfExperience;
                existingFarmer.Certifications = model.Certifications;

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Dashboard));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.FarmerProfiles.Any(e => e.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Log validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                Console.WriteLine(error); // Use your logging mechanism here
            }

            return View(model);
        }



        [HttpPost]
        public IActionResult FilterProductsByDate([FromBody] FilterProductsViewModel filter)
        {
            DateTime? startDate = string.IsNullOrEmpty(filter.StartDate) ? (DateTime?)null : DateTime.Parse(filter.StartDate);
            DateTime? stopDate = string.IsNullOrEmpty(filter.StopDate) ? (DateTime?)null : DateTime.Parse(filter.StopDate);

            var products = _context.Products
                .Include(p => p.FarmerProfile)
                .Where(p => (string.IsNullOrEmpty(filter.Category) || p.Category == filter.Category) &&
                            (!startDate.HasValue || p.ProductionDate >= startDate.Value) &&
                            (!stopDate.HasValue || p.ProductionDate <= stopDate.Value))
                .Select(p => new
                {
                    p.Name,
                    p.ImageUri,
                    p.Category,
                    ProductionDate = p.ProductionDate.ToString("yyyy-MM-dd"),
                    FarmerName = p.FarmerProfile.Name
                }).ToList();

            return Json(products);
        }
    

    public class FilterProductsViewModel
    {
        public string Category { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }
    public class Video
    {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Thumbnail { get; set; }
    }
        public IActionResult GetFarmerProducts(int farmerId)
        {
            var farmer = _context.FarmerProfiles
                .Include(f => f.Products)
                .FirstOrDefault(f => f.Id == farmerId);

            if (farmer == null)
            {
                return NotFound();
            }

            var farmerProducts = farmer.Products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Products = g.Select(p => new
                    {
                        p.Name,
                        p.ImageUri,
                        p.ProductionDate
                    })
                }).ToList();

            return Json(new { farmer.Name, ProductsByCategory = farmerProducts });
        }


        // Action to get videos (from an API or database)
        // Once clicked on the user is taken to Youtube
        // Adapted from W3Schools
        // https://www.w3schools.com/html/html_youtube.asp#:~:text=To%20play%20your%20video%20on%20a%20web%20page%2C,any%20other%20parameters%20to%20the%20URL%20%28see%20below%29
        // W3Schools
        // https://www.w3schools.com/

        public IActionResult GetVideos()
    {
        var videos = new List<Video>
        {
            new Video { Title = "Top 7 Policies Every Company Must Have", Url = "https://youtu.be/5iZzpP9Xtuc?si=7l4q2n4z-SdfcGoq", Thumbnail = "/images/vid_thumb_5.jpg" },
            new Video { Title = "Tips on Professionalism [BE A WORKPLACE STANDOUT]", Url = "https://youtu.be/iloAQmroRK0?si=nyqJxy8wuZxua-nP", Thumbnail = "/images/vid_thumb_6.jpg" },
            new Video { Title = "Intro to Farm business management and records", Url = "https://youtu.be/9ACrufWH8Io?si=NQ2fScjXRSBRB9C2", Thumbnail = "/images/vid_thumb_7.jpg" },
            new Video { Title = "PHREC PARTT Plus Keynote: South African Agriculture: Research and Technology, Dr. Johan Pretorius", Url = "https://youtu.be/NoPdbXiEMGM?si=FsqEpGkAhKapZD02", Thumbnail = "/images/vid_thumb_8.jpg" },
        };
        return Json(videos);
    }
        // Method for performing the Log-out Task
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult ViewProducts()
        {
            var products = _context.Products.Include(p => p.FarmerProfile).ToList();
            return View(products);
        }

        public IActionResult FilterProducts(string category)
        {
            var products = _context.Products
                .Include(p => p.FarmerProfile)
                .Where(p => p.Category == category).ToList();
            return View("ViewProducts", products);
        }
    }
}
