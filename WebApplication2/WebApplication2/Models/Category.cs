using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва категорії обов’язкова")]
        [StringLength(100)]
        public string Name { get; set; }

        // Зв’язок з товарами
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}