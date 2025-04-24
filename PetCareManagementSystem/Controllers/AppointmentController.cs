using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareManagementSystem.Data;
using PetCareManagementSystem.Models;

namespace PetCareManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<IdentityUser> userManager;

        public AppointmentController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.environment = environment;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var appointments = context.Appointment
                           .Select(a => new Appointment {
                               UserId = a.UserId,
                               Username = a.Username,
                               PetID = a.PetID,
                               PetName = a.PetName,
                               GroomingService = a.GroomingService,
                               Symptoms = a.Symptoms,
                               ClinicID = a.ClinicID,
                               ClinicName = a.ClinicName,
                               CityAddress = a.CityAddress,
                               Date = a.Date,
                               Time = a.Time,
                               Status = a.Status,
                               CreatedAt = a.CreatedAt
                              })
                              .ToList();

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var clinic = await context.Clinic.ToListAsync();
            var pet = await context.Pet.ToListAsync();

            var viewModel = new AddAppointmentViewModel
            {
                Clinic = clinic.Select(c => new Clinic
                {
                    ImageFileName = c.ImageFileName,
                    ClinicName = c.ClinicName,
                    CityAddress = c.CityAddress,
                    Description = c.Description
                }).ToList(),

                Pet = pet.Select(p => new Pet
                {
                    PetName = p.PetName,
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAppointmentViewModel viewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                // User is authenticated, proceed with adding the appointment
                var user = await userManager.GetUserAsync(User);
                var pet = await context.Pet.FirstOrDefaultAsync(p => p.PetName == viewModel.PetName);
                var clinic = await context.Clinic.FirstOrDefaultAsync(c => c.ImageFileName == viewModel.ImageFileName
                                                                          && c.ClinicName == viewModel.ClinicName
                                                                          && c.CityAddress == viewModel.CityAddress
                                                                          && c.Description == viewModel.Description);

                Appointment appointment = new Appointment()
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    PetID = pet.PetID,
                    PetName = pet.PetName,
                    GroomingService = viewModel.GroomingService,
                    Symptoms = viewModel.Symptoms,
                    ClinicID = clinic.ClinicID,
                    ClinicName = clinic.ClinicName,
                    CityAddress = clinic.CityAddress,
                    Date = viewModel.Date,
                    Time = viewModel.Time,
                    Status = viewModel.Status,
                    CreatedAt = DateTime.Now,
                };

                context.Appointment.Add(appointment);
                context.SaveChanges();

                TempData["Message"] = $"User {user.Id} has made a new appointment.";


                return RedirectToAction("Details", "Appointment");
            }
            else
            {

                // User is not authenticated, redirect to the login page
                return Redirect("/Identity/Account/Login");
            }
        }


        public IActionResult Details()
        {

            return View();
        }
        public IActionResult Review()
        {
            var appointment = context.Appointment.ToList();
            return View(appointment);
        }
    }
}
