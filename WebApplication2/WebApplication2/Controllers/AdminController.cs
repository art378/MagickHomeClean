using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Головна сторінка адмінки
        public IActionResult Index()
        {
            return View();
        }

        // Перегляд усіх користувачів
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        
        // GET: Управління ролями конкретного користувача
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            ViewBag.User = user;
            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = allRoles;

            return View();
        }

        // POST: Додати роль
        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(role)) return BadRequest();

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("ManageRoles", new { userId });
        }

        // POST: Забрати роль
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(role)) return BadRequest();

            await _userManager.RemoveFromRoleAsync(user, role);
            return RedirectToAction("ManageRoles", new { userId });
        }
    }
}