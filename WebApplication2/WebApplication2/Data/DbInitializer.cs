using System.Linq;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Якщо вже є категорії — пропускаємо ініціалізацію
            if (context.Categories.Any())
                return;

            // 1. Категорії
            var categories = new[]
            {
                new Category { Name = "Текстиль" },
                new Category { Name = "Посуд" },
                new Category { Name = "Декор" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // 2. Підкатегорії
            var subcategories = new[]
            {
                new Subcategory { Name = "Постільна білизна" },
                new Subcategory { Name = "Рушники" },
                new Subcategory { Name = "Гобеленові скатертини" },
                new Subcategory { Name = "Скатертини" },
                new Subcategory { Name = "Сервірування" },
                new Subcategory { Name = "Глиняний посуд" },
                new Subcategory { Name = "Скляний та порцеляновий посуд" },
                new Subcategory { Name = "Пасхальний декор" },
                new Subcategory { Name = "Картини з бурштину" },
                new Subcategory { Name = "Новорічний декор" },
                new Subcategory { Name = "Підсвічники" },
                new Subcategory { Name = "Вази" },
                new Subcategory { Name = "Фоторамки" },
                new Subcategory { Name = "Статуетки" },
            };
            context.Subcategories.AddRange(subcategories);
            context.SaveChanges();

            // 3. Товари
            var products = new[]
            {
                new Product
                {
                    Name = "Рушник махровий",
                    Description = "М'який рушник з 100% бавовни",
                    Price = 199.99M,
                    ImageUrl = "/images/rushnyk.jpg",
                    CategoryId = categories[0].Id
                },
                new Product
                {
                    Name = "Скатертина",
                    Description = "Скатертина на стіл з візерунком",
                    Price = 299.50M,
                    ImageUrl = "/images/skatertyna.jpg",
                    CategoryId = categories[0].Id
                },
                new Product
                {
                    Name = "Кружка керамічна",
                    Description = "Біла кружка об’ємом 300 мл",
                    Price = 99.00M,
                    ImageUrl = "/images/kruzhka.jpg",
                    CategoryId = categories[1].Id
                },
                new Product
                {
                    Name = "Свічка декоративна",
                    Description = "Свічка у формі квітки",
                    Price = 150.00M,
                    ImageUrl = "/images/svichka.jpg",
                    CategoryId = categories[2].Id
                }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            // 4. Зв'язки продуктів і підкатегорій
            var productSubcategories = new[]
            {
                new ProductSubcategory { ProductId = products[0].Id, SubcategoryId = subcategories.First(s => s.Name == "Рушники").Id },
                new ProductSubcategory { ProductId = products[1].Id, SubcategoryId = subcategories.First(s => s.Name == "Скатертини").Id },
                new ProductSubcategory { ProductId = products[2].Id, SubcategoryId = subcategories.First(s => s.Name == "Глиняний посуд").Id },
                new ProductSubcategory { ProductId = products[3].Id, SubcategoryId = subcategories.First(s => s.Name == "Підсвічники").Id },
            };
            context.ProductSubcategories.AddRange(productSubcategories);
            context.SaveChanges();
        }
    }
}