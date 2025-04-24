using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareManagementSystem.Models
{
    public class AddPetViewModel
    {
        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string PetName { get; set; } = "";

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Gender { get; set; } = "";

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Breed { get; set; } = "";

        [Required(ErrorMessage = "This field is required.")]
        public DateOnly Birthday { get; set; }

        public int Age { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price format. Use up to 2 decimal places.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string CurrentDiagnosis { get; set; } = "";

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Allergies { get; set; } = "";

        [MaxLength(500)]
        public string MedicalHistory { get; set; } = "";

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Neutered { get; set; } = "";

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Medications { get; set; } = "";

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Vaccinated { get; set; } = "";
    }
}
