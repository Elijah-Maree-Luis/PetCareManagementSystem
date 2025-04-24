using System.ComponentModel.DataAnnotations;

namespace PetCareManagementSystem.Models
{
    public class AddInventoryViewModel
    {
        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string ProductName { get; set; } = "";

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price format. Use up to 2 decimal places.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int Stocks { get; set; }

    }
}