using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication2.Models;
<<<<<<< HEAD
using WebApplication2.Data; // додайте!
=======
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
<<<<<<< HEAD
        private readonly ApplicationDbContext _context; // додайте!

        // Новий конструктор для отримання контексту з DI
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;  // зберігається для доступу до БД
        }

        public IActionResult Index()
        {
            return View();
        }

=======

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
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }

<<<<<<< HEAD
=======
        // 🔐 Лише для ролі Admin (опціонально)
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return View();
        }

<<<<<<< HEAD
=======
        // Сторінка помилки
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
