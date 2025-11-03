using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(string? search, int? category, int? subcategory, string? sortOrder)
        {
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSubcategories)
                    .ThenInclude(ps => ps.Subcategory)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(search));
            }

            if (subcategory.HasValue)
            {
                productsQuery = productsQuery
                    .Where(p => p.ProductSubcategories
                        .Any(ps => ps.SubcategoryId == subcategory.Value));
            }
            else if (category.HasValue)
            {
                productsQuery = productsQuery
                    .Where(p => p.CategoryId == category.Value);
            }

            // Початкове сортування за доступністю (доступні зверху)
            IOrderedQueryable<WebApplication2.Models.Product> orderedQuery = productsQuery.OrderByDescending(p => p.IsAvailable);

            // Додаємо сортування за ціною або назвою залежно від sortOrder
            if (sortOrder == "price_asc")
            {
                orderedQuery = orderedQuery.ThenBy(p => p.Price);
            }
            else if (sortOrder == "price_desc")
            {
                orderedQuery = orderedQuery.ThenByDescending(p => p.Price);
            }
            else
            {
                orderedQuery = orderedQuery.ThenBy(p => p.Name);
            }

            var products = await orderedQuery
           .Include(p => p.Discounts.Where(d => d.IsActive && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now))
           .ToListAsync();


            ViewBag.Categories = await _context.Categories.ToListAsync();

            if (category.HasValue)
            {
                ViewBag.Subcategories = await _context.Subcategories
                    .Where(sub => sub.ProductSubcategories
                        .Any(ps => ps.Product.CategoryId == category.Value))
                    .Distinct()
                    .ToListAsync();
            }
            else
            {
                ViewBag.Subcategories = await _context.Subcategories.ToListAsync();
            }

            ViewBag.SelectedCategory = category ?? null;
            ViewBag.SelectedSubcategory = subcategory ?? null;
            ViewBag.SearchTerm = search;
            ViewBag.SortOrder = sortOrder;  // передаємо для вибору сортування у View

            return View(products);
        }




        // AJAX: отримання підкатегорій за категорією
        [HttpGet]
        public async Task<JsonResult> GetSubcategoriesByCategory(int categoryId)
        {
            var subcategories = await _context.Subcategories
                .Where(sub => sub.ProductSubcategories
                    .Any(ps => ps.Product.CategoryId == categoryId))
                .Select(s => new { s.Id, s.Name })
                .Distinct()
                .ToListAsync();

            return Json(subcategories);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.AllSubcategories = _context.Subcategories.ToList();
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.SelectedSubcategoryIds = new List<int>();
            return View(new Product()); 
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<int> SelectedSubcategoryIds, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Якщо користувач завантажив фото
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var imagePath = Path.Combine("wwwroot/images/uploads", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/uploads/" + fileName;
                }

                // Прив'язка підкатегорій
                product.ProductSubcategories = SelectedSubcategoryIds
                    .Select(id => new ProductSubcategory { SubcategoryId = id })
                    .ToList();

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Якщо модель невалідна — повертаємо форму з даними
            ViewBag.AllSubcategories = _context.Subcategories.ToList();
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewBag.SelectedSubcategoryIds = new List<int>();
            return View(product);
        }


        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.ProductSubcategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.AllSubcategories = _context.Subcategories.ToList();
            ViewBag.SelectedSubcategoryIds = product.ProductSubcategories.Select(ps => ps.SubcategoryId).ToList();
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, List<int> SelectedSubcategoryIds)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);

                    var existing = _context.ProductSubcategories.Where(ps => ps.ProductId == id);
                    _context.ProductSubcategories.RemoveRange(existing);

                    product.ProductSubcategories = SelectedSubcategoryIds
                        .Select(sid => new ProductSubcategory { ProductId = id, SubcategoryId = sid })
                        .ToList();
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllSubcategories = _context.Subcategories.ToList();
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSubcategories)
                    .ThenInclude(ps => ps.Subcategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.ProductSubcategories.RemoveRange(
                    _context.ProductSubcategories.Where(ps => ps.ProductId == id));

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsAvailable = !product.IsAvailable;
            await _context.SaveChangesAsync();

            return Ok();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSubcategories)          // Додаємо завантаження підкатегорій
                    .ThenInclude(ps => ps.Subcategory)
                .Include(p => p.Discounts.Where(d => d.IsActive && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            ViewBag.FinalPrice = CalculateFinalPrice(product);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDiscount([FromBody] DiscountUpdateModel model)
        {
            if (model == null || model.Percent < 0 || model.Percent > 100)
                return BadRequest("Некоректні дані");

            var product = await _context.Products
                .Include(p => p.Discounts)
                .FirstOrDefaultAsync(p => p.Id == model.ProductId);

            if (product == null)
                return NotFound();

            // Знайти активну знижку для продукту (період актуальності можна спростити, або ставити нові дати)
            var activeDiscount = product.Discounts.FirstOrDefault(d => d.IsActive);

            if (model.IsActive)
            {
                if (activeDiscount == null)
                {
                    // Створити нову знижку
                    var discount = new Discount
                    {
                        ProductId = model.ProductId,
                        Percent = model.Percent,
                        IsActive = true,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddYears(1) // за замовчуванням 1 рік
                    };
                    _context.Discounts.Add(discount);
                }
                else
                {
                    // Оновити існуючу
                    activeDiscount.Percent = model.Percent;
                    activeDiscount.StartDate = DateTime.Now;
                    activeDiscount.EndDate = DateTime.Now.AddYears(1);
                    activeDiscount.IsActive = true;
                }
            }
            else
            {
                // Якщо знижка неактивна — вимкнути існуючу активну
                if (activeDiscount != null)
                {
                    activeDiscount.IsActive = false;
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // Допоміжний клас моделі для прийому даних з JS
        public class DiscountUpdateModel
        {
            public int ProductId { get; set; }
            public decimal Percent { get; set; }
            public bool IsActive { get; set; }
        }

        private decimal CalculateFinalPrice(Product product)
        {
            var activeDiscount = product.Discounts?.FirstOrDefault();
            if (activeDiscount != null)
            {
                return product.Price * (1 - activeDiscount.Percent / 100);
            }
            return product.Price;
        }

        // GET: Product/Database
        public async Task<IActionResult> Database()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductSubcategories)
                    .ThenInclude(ps => ps.Subcategory)
                .Include(p => p.Discounts)  // <- додати завантаження знижок
                .ToListAsync();

            return View(products);
        }


    }
}