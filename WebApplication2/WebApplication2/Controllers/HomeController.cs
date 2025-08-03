using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Головна сторінка — для всіх
        public IActionResult Index()
        {
            return View(); // 🔁 НЕ передає нічого зайвого
        }

        // 🔐 Лише для авторизованих користувачів (опціонально)
        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }

        // 🔐 Лише для ролі Admin (опціонально)
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return View();
        }

        // Сторінка помилки
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}