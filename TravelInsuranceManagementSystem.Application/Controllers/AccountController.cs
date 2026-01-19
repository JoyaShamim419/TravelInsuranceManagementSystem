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
using InsuranceClaim = TravelInsuranceManagementSystem.Application.Models.Claim;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;


namespace TravelInsuranceManagementSystem.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;

        public AccountController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        [HttpGet]
        public IActionResult SignIn() => View("~/Views/Home/SignIn.cshtml");

        [HttpPost]
        public async Task<IActionResult> SignIn(string Email, string Password)
        {
            var user = _context.Users.AsEnumerable()
                .FirstOrDefault(u => u.Email.Equals(Email, StringComparison.Ordinal));

            if (user != null)
            {
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(ClaimTypes.Name, user.FullName),
                    new System.Security.Claims.Claim(ClaimTypes.Email, user.Email),
                    new System.Security.Claims.Claim(ClaimTypes.Role, user.Role),
                    new System.Security.Claims.Claim("UserId", user.Id.ToString())
                };
                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    var token = GenerateJwtToken(user);

                    if (Request.Headers["Accept"].ToString().Contains("application/json"))
                    {
                        return Ok(new { token = token, user = user.FullName, role = user.Role });
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("UserId", user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    HttpContext.Session.SetString("JWToken", token);

                    if (user.Role == "Admin") return RedirectToAction("Dashboard", "Admin");
                    if (user.Role == "Agent") return RedirectToAction("Dashboard", "Agent");
                    return RedirectToAction("Dashboard", "UserDashboard");
                }
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
                return Unauthorized(new { message = "Invalid email or password" });

            TempData["ErrorMessage"] = "Invalid email (case-sensitive) or password!";
            return View("~/Views/Home/SignIn.cshtml");
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (_context.Users.AsEnumerable().Any(u => u.Email.Equals(user.Email, StringComparison.Ordinal)))
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                    return BadRequest(new { message = "Email already registered" });

                TempData["ErrorMessage"] = "This exact email is already registered!";
                return View("~/Views/Home/SignIn.cshtml");
            }

            user.Password = _passwordHasher.HashPassword(user, user.Password);

            if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
            else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
            else user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
                return Ok(new { message = "Registration successful" });

            return RedirectToAction("SignIn");
        }

        // --- FORGOT PASSWORD FLOW ---

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            // Case-sensitive check
            var user = _context.Users.AsEnumerable().FirstOrDefault(u => u.Email.Equals(email, StringComparison.Ordinal));

            if (user == null)
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                    return NotFound(new { message = "Email not found" });

                ViewBag.Error = "Email address not found (Case Sensitive).";
                return View();
            }

            // In a real app, you'd send an email here. For now, we redirect to reset.
            return RedirectToAction("ResetPassword", new { email = email });
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string email, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                    return BadRequest(new { message = "Passwords do not match" });

                ViewBag.Error = "Passwords do not match.";
                ViewBag.Email = email;
                return View();
            }

            var user = _context.Users.AsEnumerable().FirstOrDefault(u => u.Email.Equals(email, StringComparison.Ordinal));
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, newPassword);
                _context.Update(user);
                _context.SaveChanges();

                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                    return Ok(new { message = "Password updated successfully" });

                TempData["SuccessMessage"] = "Password reset successfully!";
                return RedirectToAction("SignIn");
            }

            return BadRequest();
        }

        // --- END FORGOT PASSWORD FLOW ---

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}