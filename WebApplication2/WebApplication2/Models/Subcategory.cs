using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Subcategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<ProductSubcategory> ProductSubcategories { get; set; } = new List<ProductSubcategory>();
    }
}