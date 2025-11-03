using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models; // Замініть на ваш namespace

namespace WebApplication2.Controllers // Замініть на ваш namespace
{
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var discounts = _context.Discounts
                .Include(d => d.Product)
                .Where(d => d.EndDate >= DateTime.Now)
                .ToList();
            return View(discounts);
        }

        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Discount model)
        {
            if (ModelState.IsValid)
            {
                _context.Discounts.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Products = _context.Products.ToList();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var discount = _context.Discounts.Find(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
