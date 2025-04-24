using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.BuilderProperties;

namespace PetCareManagementSystem.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }


        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [MaxLength(100)]
        public string Username { get; set; } = "";


        public int PetID { get; set; }

        public Pet Pet { get; set; }

        [MaxLength(100)]
        public string PetName { get; set; } = "";


        [MaxLength(100)]
        public string GroomingService { get; set; } = "";

        [MaxLength(500)]
        public string Symptoms { get; set; } = "";


        public int ClinicID { get; set; }

        public Clinic Clinic { get; set; }

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        [MaxLength(100)]
        public string ClinicName { get; set; } = "";

        [MaxLength(100)]
        public string CityAddress { get; set; } = "";

        [MaxLength(500)]
        public string Description { get; set; } = "";


        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        [MaxLength(100)]
        public string Status { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
