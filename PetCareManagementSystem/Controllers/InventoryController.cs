using Microsoft.AspNetCore.Mvc;
using PetCareManagementSystem.Data;
using PetCareManagementSystem.Models;

namespace PetCareManagementSystem.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public InventoryController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var products = context.Inventory.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddInventoryViewModel viewModel)
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

            string filePath = environment.WebRootPath + "/product/" + newFileName;
            using (var stream = System.IO.File.Create(filePath))
            {
                viewModel.ImageFile.CopyTo(stream);
            }

            Inventory product = new Inventory()
            {
                ProductName = viewModel.ProductName,
                ImageFileName = newFileName,
                Description = viewModel.Description,
                Price = viewModel.Price,
                Stocks = viewModel.Stocks,
                CreatedAt = DateTime.Now,
            };

            context.Inventory.Add(product);
            context.SaveChanges();

            TempData["Message"] = $"ProductID {product.ProductID} successfully added.";
            return RedirectToAction("Index", "Inventory");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Inventory.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Inventory");
            }

            var viewModel = new AddInventoryViewModel()
            {
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Stocks = product.Stocks,
            };

            ViewData["ProductID"] = product.ProductID;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, AddInventoryViewModel viewModel)
        {
            var product = context.Inventory.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Inventory");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductID"] = product.ProductID;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(viewModel);
            }

            string newFileName = product.ImageFileName;
            if (viewModel.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(viewModel.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/product/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    viewModel.ImageFile.CopyTo(stream);
                }

                string oldImageFullPath = environment.WebRootPath + "/product/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            product.ProductName = viewModel.ProductName;
            product.ImageFileName = newFileName;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.Stocks = viewModel.Stocks;

            context.SaveChanges();

            TempData["Message"] = $"ProductID {product.ProductID}'s details successfully updated.";
            return RedirectToAction("Index", "Inventory");
        }

        public IActionResult Delete(int id)
        {
            var product = context.Inventory.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Inventory");
            }

            string imageFullPath = environment.WebRootPath + "/product/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Inventory.Remove(product);
            context.SaveChanges(true);

            TempData["Message"] = $"ProductID {product.ProductID} successfully deleted.";
            return RedirectToAction("Index", "Inventory");
        }
    }
}