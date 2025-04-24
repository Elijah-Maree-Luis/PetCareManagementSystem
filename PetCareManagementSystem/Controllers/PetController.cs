using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetCareManagementSystem.Data;
using PetCareManagementSystem.Models;
using System.Security.Claims;

namespace PetCareManagementSystem.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<IdentityUser> userManager;

        public PetController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.environment = environment;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var pets = context.Pet.ToList();
            return View(pets);
        }

        public IActionResult PetDetails()
        {
            var pets = context.Pet.ToList();
            return View(pets);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPetViewModel viewModel)
        {
            if (viewModel.ImageFile == null || viewModel.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "This field is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(viewModel.ImageFile!.FileName);

            string filePath = Path.Combine(environment.WebRootPath, "pet", newFileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                viewModel.ImageFile.CopyTo(stream);
            }

            var user = await userManager.GetUserAsync(User);

            Pet pet = new Pet()
            {
                PetName = viewModel.PetName,
                ImageFileName = newFileName,
                Gender = viewModel.Gender,
                Breed = viewModel.Breed,
                Birthday = viewModel.Birthday,
                Age = viewModel.Age,
                Weight = viewModel.Weight,
                CurrentDiagnosis = viewModel.CurrentDiagnosis,
                Allergies = viewModel.Allergies,
                MedicalHistory = viewModel.MedicalHistory,
                Neutered = viewModel.Neutered,
                Medications = viewModel.Medications,
                Vaccinated = viewModel.Vaccinated,
                CreatedAt = DateTime.Now,
                UserId = user.Id // Set the UserId to the logged-in user's ID
            };

            context.Pet.Add(pet);
            context.SaveChanges();

            TempData["Message"] = $"New!: {pet.PetID} has been registered in the system.";
            return RedirectToAction("PetDetails", "Pet");
        }

        public IActionResult Edit(int id)
        {
            var pet = context.Pet.Find(id);

            if (pet == null)
            {
                return RedirectToAction("PetDetails", "Pet");
            }

            var viewModel = new AddPetViewModel()
            {
                PetName = pet.PetName,
                Gender = pet.Gender,
                Breed = pet.Breed,
                Birthday = pet.Birthday,
                Age = pet.Age,
                Weight = pet.Weight,
                CurrentDiagnosis = pet.CurrentDiagnosis,
                Allergies = pet.Allergies,
                MedicalHistory = pet.MedicalHistory,
                Neutered = pet.Neutered,
                Medications = pet.Medications,
                Vaccinated = pet.Vaccinated,
            };

            ViewData["PetID"] = pet.PetID;
            ViewData["ImageFileName"] = pet.ImageFileName;
            ViewData["CreatedAt"] = pet.CreatedAt.ToString("MM/dd/yyyy");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, AddPetViewModel viewModel)
        {
            var pet = context.Pet.Find(id);

            if (pet == null)
            {
                return RedirectToAction("PetDetails", "Pet");
            }

            if (!ModelState.IsValid)
            {
                ViewData["PetID"] = pet.PetID;
                ViewData["ImageFileName"] = pet.ImageFileName;
                ViewData["CreatedAt"] = pet.CreatedAt.ToString("MM/dd/yyyy");

                return View(viewModel);
            }

            string newFileName = pet.ImageFileName;
            if (viewModel.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(viewModel.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/pet/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    viewModel.ImageFile.CopyTo(stream);
                }

                string oldImageFullPath = environment.WebRootPath + "/pet/" + pet.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            pet.ImageFileName = newFileName;
            pet.PetName = viewModel.PetName;
            pet.Gender = viewModel.Gender;
            pet.Breed = viewModel.Breed;
            pet.Birthday = viewModel.Birthday;
            pet.Age = viewModel.Age;
            pet.Weight = viewModel.Weight;
            pet.CurrentDiagnosis = viewModel.CurrentDiagnosis;
            pet.Allergies = viewModel.Allergies;
            pet.MedicalHistory = viewModel.MedicalHistory;
            pet.Neutered = viewModel.Neutered;
            pet.Medications = viewModel.Medications;
            pet.Vaccinated = viewModel.Vaccinated;

            {
                DateTime entryDate = DateTime.Now;

                string medicalHistoryEntry = ConstructMedicalHistoryEntry(entryDate, viewModel, pet);
                var entries = (medicalHistoryEntry + "\n" + pet.MedicalHistory).Split('\n');
                var recentEntries = entries.Take(3);
                pet.MedicalHistory = string.Join("\n", recentEntries);
            }

            context.SaveChanges();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Accessing user ID from Claims
            TempData["Message"] = $"User {userId} has updated the profile of PetID {pet.PetID}.";
            return RedirectToAction("PetDetails", "Pet");
        }

        private string ConstructMedicalHistoryEntry(DateTime entryDate, AddPetViewModel viewModel, Pet pet)
        {
            string medicalHistoryEntry = $"• {entryDate.ToString("MM/dd/yyyy")} - Weight: {viewModel.Weight}, Current Diagnosis: {viewModel.CurrentDiagnosis}, Allergies: {viewModel.Allergies}, Neutered: {viewModel.Neutered}, Medications: {viewModel.Medications}, Vaccinated: {viewModel.Vaccinated}";

            return medicalHistoryEntry;
        }

        public IActionResult Delete(int id)
        {
            var pet = context.Pet.Find(id);
            if (pet == null)
            {
                return RedirectToAction("PetDetails", "Pet");
            }

            string imageFullPath = environment.WebRootPath + "/pet/" + pet.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Pet.Remove(pet);
            context.SaveChanges(true);

            TempData["Message"] = $"{pet.PetID}'s profile was deleted.";
            return RedirectToAction("PetDetails", "Pet");
        }
    }
}
