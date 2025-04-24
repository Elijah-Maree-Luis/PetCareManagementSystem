using System.ComponentModel.DataAnnotations;

namespace PetCareManagementSystem.Models
{
    public class AddAppointmentViewModel
    {
        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Username { get; set; } = "";


        public List<Pet> Pet { get; set; }

        public int PetID { get; set; }

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string PetName { get; set; } = "";


        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string GroomingService { get; set; } = "";

        [Required(ErrorMessage = "This field is required."), MaxLength(500)]
        public string Symptoms { get; set; } = "";


        public List<Clinic> Clinic { get; set; }

        public int ClinicID { get; set; }

        public string ImageFileName { get; set; } = "";

        [MaxLength(100)]
        public string ClinicName { get; set; } = "";

        [MaxLength(100)]
        public string CityAddress { get; set; } = "";

        [MaxLength(500)]
        public string Description { get; set; } = "";


        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        [Required(ErrorMessage = "This field is required."), MaxLength(100)]
        public string Status { get; set; } = "";
    }
}


