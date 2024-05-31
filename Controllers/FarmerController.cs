using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Controllers
{
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly SignInManager<User> _signInManager;



        public FarmerController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, SignInManager<User> signInManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _signInManager = signInManager;
        }

        public class Video
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Thumbnail { get; set; }
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
                new Video { Title = "How to Start a Small Farm | A Step-by-Step Guide", Url = "https://youtu.be/heTxEsrPVdQ?si=yGXLG1KY160ZNd5f", Thumbnail = "/images/vid_thumb_1.jpg" },
                new Video { Title = "What is Sustainable Agriculture? Episode 1: A Whole-Farm Approach to Sustainability", Url = "https://youtu.be/iloAQmroRK0?si=nyqJxy8wuZxua-nP", Thumbnail = "/images/vid_thumb_2.jpg" },
                new Video { Title = "Solar Panels Plus Farming? Agrivoltaics Explained", Url = "https://youtu.be/lgZBlD-TCFE?si=oyaedn5L88VpTo5f", Thumbnail = "/images/vid_thumb_3.jpg" },
                new Video { Title = "How Agrivoltaic Farming Can Help Solve Our Food & Energy Crises..", Url = "https://youtu.be/TT-qgpc4VHk?si=7x8wY_ZHleFLXYYe", Thumbnail = "/images/vid_thumb_4.jpg" },
            };
            return Json(videos);
        }
        // Method for performing the Log-out Task
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }
        public async Task<IActionResult> Dashboard()
        {
            // Get the ID of the currently logged-in user
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the necessary data for the dashboard view
            var products = await _context.Products
                                         .Include(p => p.FarmerProfile)
                                         .Where(p => p.FarmerProfile.UserId.ToString() == currentUserId)
                                         .ToListAsync();

            var groupedProducts = products.GroupBy(p => p.Category).ToList();

            return View(groupedProducts);
        }



        public IActionResult AddProduct()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile ImageUri)
        {
            if (ModelState.IsValid)
            {
                // Get the ID of the currently logged-in user
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Find the FarmerProfile associated with the current user ID
                var farmerProfile = await _context.FarmerProfiles
                                                  .FirstOrDefaultAsync(fp => fp.UserId.ToString() == currentUserId);

                if (farmerProfile != null)
                {
                    if (ImageUri != null)
                    {
                        // Get the filename and extension
                        var fileName = Path.GetFileName(ImageUri.FileName);
                        var extension = Path.GetExtension(fileName);

                        // Generate a unique filename to prevent conflicts
                        var uniqueFileName = Guid.NewGuid().ToString() + extension;

                        // Get the directory where images will be stored
                        var uploadsDir = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        // Create the directory if it doesn't exist
                        if (!Directory.Exists(uploadsDir))
                        {
                            Directory.CreateDirectory(uploadsDir);
                        }

                        // Combine the directory and the filename to get the full path
                        var filePath = Path.Combine(uploadsDir, uniqueFileName);

                        // Copy the uploaded file to the specified path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageUri.CopyToAsync(stream);
                        }

                        // Set the ImageUri property of the product
                        product.ImageUri = "/images/" + uniqueFileName;
                    }

                    // Associate the product with the found FarmerProfile
                    product.FarmerProfileId = farmerProfile.Id;

                    // Add the product to the context and save changes
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    // Handle case where FarmerProfile for the current user was not found
                    ModelState.AddModelError(string.Empty, "Farmer profile not found for current user.");
                }
            }

            return View(product);
        }
    }


}



