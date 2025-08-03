namespace WebApplication2.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;
    using WebApplication2.Data;
    using WebApplication2.Models;

    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartKey = "cart";

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checkout
        public IActionResult Checkout()
        {
            var cart = GetCartFromSession();
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            return View(new Order());
        }

        // POST: Checkout
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var cart = GetCartFromSession();

            if (!ModelState.IsValid || !cart.Any())
            {
                return View(order);
            }
            var newOrder = new Order
            {
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber, // Заміна Email на PhoneNumber
                Address = order.Address,
                CreatedAt = DateTime.Now,
                Items = cart.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            SaveCartToSession(new List<CartItem>());
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        // ---------------------
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
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Items)
                .ToList();

            return View(orders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.OrderItems.RemoveRange(order.Items);
            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, OrderStatus status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            order.Status = status;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
