using System.ComponentModel.DataAnnotations;

namespace PetCareManagementSystem.Models
{
    public class AddClinicViewModel
    {
        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string ClinicName { get; set; } = "";
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string CityAddress { get; set; } = "";

        [Required(ErrorMessage = "This field is required."), MaxLength(500)]
        public string Description { get; set; } = "";
    }
}
