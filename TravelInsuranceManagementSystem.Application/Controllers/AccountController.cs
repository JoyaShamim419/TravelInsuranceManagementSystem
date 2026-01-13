using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SignIn() => View("~/Views/Home/SignIn.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(string Email, string Password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

            // Verify Password using BCrypt
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                var claims = new List<Claim>
                {
                    // FIX: ClaimTypes.Name MUST be the Email for the Dashboard lookup to work
                    new Claim(ClaimTypes.Name, user.Email), 
                    new Claim("FullName", user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                    });

                // Role-based redirection logic
                if (user.Role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");

                if (user.Role == "Agent")
                    return RedirectToAction("Dashboard", "Agent");

                return RedirectToAction("Dashboard", "UserDashboard");
            }

            TempData["ErrorMessage"] = "Invalid email or password!";
            return View("~/Views/Home/SignIn.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                TempData["ErrorMessage"] = "Email already registered!";
                return View("~/Views/Home/SignIn.cshtml");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
            else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
            else user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("SignIn");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}