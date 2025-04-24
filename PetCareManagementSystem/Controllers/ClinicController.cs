using Microsoft.AspNetCore.Mvc;
using PetCareManagementSystem.Data;
using PetCareManagementSystem.Models;

namespace PetCareManagementSystem.Controllers
{
    public class ClinicController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ClinicController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var clinics = context.Clinic.ToList();
            return View(clinics);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddClinicViewModel viewModel)
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

            string filePath = environment.WebRootPath + "/clinic/" + newFileName;
            using (var stream = System.IO.File.Create(filePath))
            {
                viewModel.ImageFile.CopyTo(stream);
            }

            Clinic clinic = new Clinic()
            {
                ImageFileName = newFileName,
                ClinicName = viewModel.ClinicName,
                CityAddress = viewModel.CityAddress,
                Description = viewModel.Description,
                CreatedAt = DateTime.Now,
            };

            context.Clinic.Add(clinic);
            context.SaveChanges();

            TempData["Message"] = $"ClinicID {clinic.ClinicID} successfully added.";
            return RedirectToAction("Index", "Clinic");
        }

        public IActionResult Edit(int id)
        {
            var clinic = context.Clinic.Find(id);

            if (clinic == null)
            {
                return RedirectToAction("Index", "Clinic");
            }

            var viewModel = new AddClinicViewModel()
            {
                ClinicName = clinic.ClinicName,
                CityAddress = clinic.CityAddress,
                Description = clinic.Description,
            };

            ViewData["ClinicID"] = clinic.ClinicID;
            ViewData["ImageFileName"] = clinic.ImageFileName;
            ViewData["CreatedAt"] = clinic.CreatedAt.ToString("MM/dd/yyyy");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, AddClinicViewModel viewModel)
        {
            var clinic = context.Clinic.Find(id);

            if (clinic == null)
            {
                return RedirectToAction("Index", "Clinic");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ClinicID"] = clinic.ClinicID;
                ViewData["ImageFileName"] = clinic.ImageFileName;
                ViewData["CreatedAt"] = clinic.CreatedAt.ToString("MM/dd/yyyy");

                return View(viewModel);
            }

            string newFileName = clinic.ImageFileName;
            if (viewModel.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(viewModel.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/clinic/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    viewModel.ImageFile.CopyTo(stream);
                }

                string oldImageFullPath = environment.WebRootPath + "/clinic/" + clinic.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            clinic.ImageFileName = newFileName;
            clinic.ClinicName = viewModel.ClinicName;
            clinic.CityAddress = viewModel.CityAddress;
            clinic.Description = viewModel.Description;

            context.SaveChanges();

            TempData["Message"] = $"ClinicID {clinic.ClinicID}'s information successfully updated.";
            return RedirectToAction("Index", "Clinic");
        }

        public IActionResult Delete(int id)
        {
            var clinic = context.Clinic.Find(id);
            if (clinic == null)
            {
                return RedirectToAction("Index", "Clinic");
            }

            string imageFullPath = environment.WebRootPath + "/clinic/" + clinic.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Clinic.Remove(clinic);
            context.SaveChanges(true);

            TempData["Message"] = $"ClinicID {clinic.ClinicID} successfully deleted.";
            return RedirectToAction("Index", "Clinic");
        }
    }
}
