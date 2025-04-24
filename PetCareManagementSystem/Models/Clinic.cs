using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetCareManagementSystem.Models
{
    public class Clinic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClinicID { get; set; }

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        [MaxLength(100)]
        public string ClinicName { get; set; } = "";

        [MaxLength(100)]
        public string CityAddress { get; set; } = "";

        [MaxLength(500)]
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
