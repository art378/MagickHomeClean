namespace WebApplication2.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;
    using WebApplication2.Data;
    using WebApplication2.Models;

    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartKey = "cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Показати кошик
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCartFromSession();
            var count = cart.Sum(i => i.Quantity);
            return Json(new { count });
        }

        // Додати товар до кошика
        [HttpPost]
<<<<<<< HEAD
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AddToCart(int id, string SelectedSize, decimal SelectedPrice)
=======
        public JsonResult AddToCart(int id)
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
        {
            var product = _context.Products
                .Include(p => p.Discounts)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return Json(new { success = false, message = "Товар не знайдено" });

            var activeDiscount = product.Discounts
                .FirstOrDefault(d => d.IsActive && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now);

            var cart = GetCartFromSession();

            var item = cart.FirstOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
<<<<<<< HEAD
                    DiscountPercent = activeDiscount?.Percent, // може бути null
                    Quantity = 1,
                    ImageUrl = product.ImageUrl,
                    Size = SelectedSize,
                    SizePrice = SelectedPrice, // ← так!
                    OriginalPrice = product.Price, // для історії/знижки
=======
                    OriginalPrice = product.Price, // ✅ правильне поле
                    DiscountPercent = activeDiscount?.Percent, // може бути null
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
                });
            }

            SaveCartToSession(cart);

            var totalItems = cart.Sum(i => i.Quantity);
            HttpContext.Session.SetInt32("CartCount", totalItems);

            return Json(new { success = true, totalItems });
        }

        // Очистити кошик
        public IActionResult Clear()
        {
            SaveCartToSession(new List<CartItem>());
            return RedirectToAction("Index");
        }

        // --------------------
        // Методи для роботи з сесією
        private List<CartItem> GetCartFromSession()
        {
            var sessionData = HttpContext.Session.GetString(CartKey);
            return sessionData != null
                ? JsonSerializer.Deserialize<List<CartItem>>(sessionData)!
                : new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(cart);

                // Оновити лічильник в сесії
                var totalItems = cart.Sum(i => i.Quantity);
                HttpContext.Session.SetInt32("CartCount", totalItems);
            }

            return RedirectToAction("Index");
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
