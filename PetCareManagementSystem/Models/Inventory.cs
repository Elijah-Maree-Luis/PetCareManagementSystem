using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetCareManagementSystem.Models
{
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [MaxLength(100)]
        public string ProductName { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        public string Description { get; set; } = "";

        [Precision(16, 2)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price format. Use up to 2 decimal places.")]
        public decimal Price { get; set; }

        public int Stocks { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
