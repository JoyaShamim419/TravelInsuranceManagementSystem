////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using TravelInsuranceManagementSystem.Application.Data;
////using TravelInsuranceManagementSystem.Application.Models;

////namespace TravelInsuranceManagementSystem.Application.Controllers
////{
////    public class AccountController : Controller
////    {
////        private readonly ApplicationDbContext _context;

////        public AccountController(ApplicationDbContext context)
////        {
////            _context = context;
////        }



////        [HttpPost]
////        public IActionResult SignIn(string Email, string Password)
////        {
////            var user = _context.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

////            if (user != null)
////            {
////                // Your logic: Route based on email contents
////                if (user.Email.ToLower().Contains("@admin"))
////                    return RedirectToAction("Dashboard", "Admin");
////                if (user.Email.ToLower().Contains("@agent"))
////                    return RedirectToAction("Dashboard", "Agent");

////                return RedirectToAction("Dashboard", "UserDashboard");
////            }

////            ModelState.AddModelError("", "Invalid email or password!");
////            return View("~/Views/Home/SignIn.cshtml");
////        }

////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public IActionResult SignUp(User user)
////        {
////            if (ModelState.IsValid)
////            {
////                // Logic: Assign role based on email domain
////                if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
////                else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
////                else user.Role = "User";

////                _context.Users.Add(user);
////                _context.SaveChanges();
////                return RedirectToAction("SignIn", "Home");
////            }
////            return View("~/Views/Home/SignIn.cshtml");
////        }
////    }
////}

////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.AspNetCore.Authentication;
////using Microsoft.AspNetCore.Authentication.Cookies;
////using System.Security.Claims;
////using TravelInsuranceManagementSystem.Application.Data;
////using TravelInsuranceManagementSystem.Application.Models;
////using BCrypt.Net;

////namespace TravelInsuranceManagementSystem.Application.Controllers
////{
////    public class AccountController : Controller
////    {
////        private readonly ApplicationDbContext _context;

////        public AccountController(ApplicationDbContext context)
////        {
////            _context = context;
////        }

////        [HttpGet]
////        public IActionResult SignIn()
////        {
////            return View("~/Views/Home/SignIn.cshtml");
////        }

////        [HttpPost]
////        public async Task<IActionResult> SignIn(string Email, string Password)
////        {
////            // 1. Find user by Email only
////            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

////            // 2. Verify hashed password
////            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
////            {
////                // 3. Create Identity Claims (The "Token" data)
////                var claims = new List<Claim>
////                {
////                    new Claim(ClaimTypes.Name, user.FullName),
////                    new Claim(ClaimTypes.Email, user.Email),
////                    new Claim(ClaimTypes.Role, user.Role),
////                    new Claim("UserId", user.Id.ToString())
////                };

////                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

////                // 4. Issue the Authentication Cookie
////                await HttpContext.SignInAsync(
////                    CookieAuthenticationDefaults.AuthenticationScheme,
////                    new ClaimsPrincipal(claimsIdentity),
////                    new AuthenticationProperties { IsPersistent = true });

////                // 5. Route based on role
////                if (user.Role == "Admin")
////                    return RedirectToAction("Dashboard", "Admin");
////                if (user.Role == "Agent")
////                    return RedirectToAction("Dashboard", "Agent");

////                return RedirectToAction("Index", "Home");
////            }

////            TempData["ErrorMessage"] = "Invalid email or password!";
////            return View("~/Views/Home/SignIn.cshtml");
////        }

////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public IActionResult SignUp(User user)
////        {
////            if (ModelState.IsValid)
////            {
////                // Check if user already exists
////                if (_context.Users.Any(u => u.Email == user.Email))
////                {
////                    ModelState.AddModelError("Email", "Email already exists.");
////                    return View("~/Views/Home/SignIn.cshtml");
////                }

////                // --- ADDED: Hash Password before saving ---
////                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

////                // Role Logic
////                if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
////                else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
////                else user.Role = "User";

////                _context.Users.Add(user);
////                _context.SaveChanges();

////                return RedirectToAction("SignIn");
////            }
////            return View("~/Views/Home/SignIn.cshtml");
////        }

////        // --- ADDED: Logout Logic ---
////        public async Task<IActionResult> Logout()
////        {
////            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
////            return RedirectToAction("Index", "Home");
////        }
////    }
////}



//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using System.Security.Claims;
//using TravelInsuranceManagementSystem.Application.Data;
//using TravelInsuranceManagementSystem.Application.Models;

//namespace TravelInsuranceManagementSystem.Application.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public AccountController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public IActionResult SignIn() => View("~/Views/Home/SignIn.cshtml");

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> SignIn(string Email, string Password)
//        {
//            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

//            // Verify Password using BCrypt
//            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
//            {
//                var claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, user.FullName),
//                    new Claim(ClaimTypes.Email, user.Email),
//                    new Claim(ClaimTypes.Role, user.Role),
//                    new Claim("UserId", user.Id.ToString())
//                };

//                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//                await HttpContext.SignInAsync(
//                    CookieAuthenticationDefaults.AuthenticationScheme,
//                    new ClaimsPrincipal(claimsIdentity));

//                // ROLE BASED REDIRECTION
//                if (user.Role == "Admin")
//                    return RedirectToAction("Dashboard", "Admin"); // Ensure AdminController exists

//                if (user.Role == "Agent")
//                    return RedirectToAction("Dashboard", "Agent"); // Ensure AgentController exists

//                return RedirectToAction("Dashboard", "UserDashboard");
//            }

//            TempData["ErrorMessage"] = "Invalid email or password!";
//            return View("~/Views/Home/SignIn.cshtml");
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult SignUp(User user)
//        {
//            if (_context.Users.Any(u => u.Email == user.Email))
//            {
//                TempData["ErrorMessage"] = "Email already registered!";
//                return View("~/Views/Home/SignIn.cshtml");
//            }

//            // 1. Hash Password
//            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

//            // 2. Assign Role based on Email
//            if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
//            else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
//            else user.Role = "User";

//            // 3. Save to DB
//            _context.Users.Add(user);
//            _context.SaveChanges();

//            return RedirectToAction("SignIn");
//        }

//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            return RedirectToAction("Index", "Home");
//        }

//        public IActionResult AccessDenied() => View();
//    }
//}

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
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user and create the authentication cookie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true, // Persistent cookie across browser sessions
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60) // Session expiry
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

            // 1. Hash Password before saving to database
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // 2. Assign Role automatically based on Email domain
            if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
            else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
            else user.Role = "User";

            // 3. Save User to Database
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("SignIn");
        }

        // FIXED LOGOUT: This explicitly clears the authentication cookie
        public async Task<IActionResult> Logout()
        {
            // Clear the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the Public Home Page
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}