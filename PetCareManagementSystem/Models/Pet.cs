using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PetCareManagementSystem.Models
{
    public class Pet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetID { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [MaxLength(100)]
        public string PetName { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        [MaxLength(100)]
        public string Gender { get; set; } = "";

        [MaxLength(100)]
        public string Breed { get; set; } = "";

        public DateOnly Birthday { get; set; }

        public int Age { get; set; }

        [Precision(16, 2)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price format. Use up to 2 decimal places.")]
        public decimal Weight { get; set; }

        [MaxLength(100)]
        public string CurrentDiagnosis { get; set; } = "";

        [MaxLength(100)]
        public string Allergies { get; set; } = "";

        [MaxLength(500)]
        public string MedicalHistory { get; set; } = "";

        [MaxLength(100)]
        public string Neutered { get; set; } = "";

        [MaxLength(100)]
        public string Medications { get; set; } = "";

        [MaxLength(100)]
        public string Vaccinated { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
