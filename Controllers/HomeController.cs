using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Data;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;


    public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginModel, RegisterViewModel registerModel, string actionType)
    {
        if (actionType == "login")
        {
              var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginModel.Email);
                    if (user != null)
                    {
                        if (user.Role == "farmer")
                        {
                            return RedirectToAction("Dashboard", "Farmer");
                        }
                        else if (user.Role == "employee")
                        {
                            return RedirectToAction("Dashboard", "Employee");
                        }
                    }
                }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginModel);
        }

        else if (actionType == "register")
        {
                var user = new User { UserName = registerModel.Email, Email = registerModel.Email, Role = registerModel.Role };
                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    // Save the user first
                    await _context.SaveChangesAsync();

                    // If the user is a farmer, save the farmer profile
                    if (registerModel.Role == "farmer")
                    {
                        string imagePath = null;
                        if (registerModel.Image != null)
                        {
                        // Get the unique file name
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(registerModel.Image.FileName);

                        // Define the path where the image will be stored
                        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                        // Copy the image to the specified path
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await registerModel.Image.CopyToAsync(fileStream);
                        }
                        imagePath = "/images/" + fileName;
                        var farmerProfile = new FarmerProfile
                        {
                            Name = registerModel.Name,
                            Location = registerModel.Location,
                            ContactInfo = registerModel.ContactInfo,
                            UserId = user.Id, // Associate the FarmerProfile with the User
                            // Set the image path in the registerModel
                            ImagePath = imagePath,
                            Certifications = "",
                            FarmSize = 0,
                            TypeOfFarming = "",
                            YearsOfExperience = 0,
                        };
                       
                        _context.FarmerProfiles.Add(farmerProfile);
                    }
                    else
                    {
                        var farmerProfile = new FarmerProfile
                        {
                            Name = registerModel.Name,
                            Location = registerModel.Location,
                            ContactInfo = registerModel.ContactInfo,
                            UserId = user.Id, // Associate the FarmerProfile with the User
                                              // Set the image path in the registerModel
                            ImagePath = registerModel.ImagePath = "/images/default_image.png"
                        };

                        _context.FarmerProfiles.Add(farmerProfile);
                    }

                }
                    // If the user is an employee, save the employee profile
                    else if (registerModel.Role == "employee")
                    {
                        var employeeProfile = new EmployeeProfile
                        {
                            Department = registerModel.Department,
                            Position = registerModel.Position,
                            UserId = user.Id // Associate the EmployeeProfile with the User
                        };

                        _context.EmployeeProfiles.Add(employeeProfile);
                    }

                    // Save all changes including FarmerProfile or EmployeeProfile if applicable
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (user.Role == "farmer")
                    {
                        return RedirectToAction("Dashboard", "Farmer");
                    }
                    else if (user.Role == "employee")
                    {
                        return RedirectToAction("Dashboard", "Employee");
                    }
                }
                AddErrors(result);
            return View(registerModel);
        }

        return View();
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
