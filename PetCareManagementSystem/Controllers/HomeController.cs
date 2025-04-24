using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCareManagementSystem.Models;
using System.Diagnostics;
using System.Security.Claims;
using PetCareManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using System;


namespace PetCareManagementSystem.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }
        public IActionResult Shop()
        {
            ViewBag.ActivePage = "Shop";
            var products = _dbContext.Inventory.ToList();
            return View(products);
         
        }

        public IActionResult Index()
        {
           
            ViewBag.ActivePage = "Home";
            return View();
        }



        public IActionResult Appointment()
        {
           
            ViewBag.ActivePage = "Appointment";
            return View();
        }

        public IActionResult About()
        {
            
            ViewBag.ActivePage = "About";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Article()
        {
            ViewBag.ActivePage = "Article";
            return View();
        }

        public IActionResult PetDetails()
        {
        
            return View();
        }

        public IActionResult HelpCenter()
        {
            
            ViewBag.ActivePage = "About";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View("~/Views/Dashboard/Dashboard.cshtml");
        }
        public IActionResult Appointments()
        {
            return View("~/Views/Dashboard/Appointments.cshtml");
        }

        public IActionResult Inventory()
        {
            return View("~/Views/Dashboard/Inventory.cshtml");
        }

        public IActionResult PetInfo()
        {
            return View("~/Views/Dashboard/PetInfo.cshtml");
        }
        public IActionResult UsersInfo()
        {
            return View("~/Views/Dashboard/UsersInfo.cshtml");
        }



        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
           
            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(username);

                if (user != null)
                {
                    if (await _signInManager.UserManager.IsInRoleAsync(user, "Admin"))
                    {
                        // Redirect the admin user to the Dashboard action
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        // Redirect regular users to a different page (e.g., their profile)
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // User not found
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return RedirectToPage("/Account/Login", new { area = "Identity", message = "User not found" });
                }
            }
            else
            {
                // If sign-in fails, return the login view with an error
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return RedirectToPage("/Account/Login", new { area = "Identity", message = "Invalid login attempt." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
