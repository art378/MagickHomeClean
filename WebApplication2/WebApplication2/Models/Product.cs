using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва обов’язкова")]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Ціна має бути від 0.01 до 10000")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Оберіть категорію")]
        public int CategoryId { get; set; }

        // Навігаційна властивість до основної категорії
        public Category? Category { get; set; }
        [NotMapped] // щоб EF Core не враховував це поле у базі
        public IFormFile? ImageFile { get; set; }

        // НОВЕ: Зв’язок багато-до-багатьох із підкатегоріями
        public ICollection<ProductSubcategory> ProductSubcategories { get; set; } = new List<ProductSubcategory>();
        public bool IsAvailable { get; set; } = true; // За замовчуванням товар доступний
        public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
    }
}