using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;

// Alias to avoid conflict with Controller.User
using AppUser = TravelInsuranceManagementSystem.Application.Models.User;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IConfiguration _config;

        public AccountController(ApplicationDbContext context, IPasswordHasher<AppUser> passwordHasher, IConfiguration config)
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
            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

            if (user != null)
            {
                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    // MODIFIED: Ensure UserId is included in the Cookie Claims
                    var authClaims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(ClaimTypes.Name, user.FullName),
                        new System.Security.Claims.Claim(ClaimTypes.Email, user.Email),
                        new System.Security.Claims.Claim(ClaimTypes.Role, user.Role),
                        new System.Security.Claims.Claim("UserId", user.Id.ToString())
                    };

                    var token = GenerateJwtToken(user);

                    if (Request.Headers["Accept"].ToString().Contains("application/json"))
                    {
                        return Ok(new { token = token, user = user.FullName, role = user.Role });
                    }

                    var claimsIdentity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    HttpContext.Session.SetString("JWToken", token);

                    if (user.Role == "Admin") return RedirectToAction("Dashboard", "Admin");
                    if (user.Role == "Agent") return RedirectToAction("Dashboard", "Agent");

                    return RedirectToAction("Dashboard", "UserDashboard");
                }
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
                return Unauthorized(new { message = "Invalid email or password" });

            TempData["ErrorMessage"] = "Invalid email or password!";
            return View("~/Views/Home/SignIn.cshtml");
        }

        [HttpPost]
        public IActionResult SignUp(AppUser user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                TempData["ErrorMessage"] = "This email is already registered!";
                return View("~/Views/Home/SignIn.cshtml");
            }

            user.Password = _passwordHasher.HashPassword(user, user.Password);

            if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
            else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
            else user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Email address not found.";
                return View();
            }
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
                ViewBag.Error = "Passwords do not match.";
                ViewBag.Email = email;
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, newPassword);
                _context.Update(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Password reset successfully!";
                return RedirectToAction("SignIn");
            }

            return BadRequest();
        }

        // --- JWT GENERATION ---

        private string GenerateJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string jwtKeyString = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
            var key = Encoding.ASCII.GetBytes(jwtKeyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // MODIFIED: Added UserId to the JWT Subject so it is available to API calls
                Subject = new ClaimsIdentity(new[] {
                    new System.Security.Claims.Claim(ClaimTypes.Name, user.FullName),
                    new System.Security.Claims.Claim(ClaimTypes.Email, user.Email),
                    new System.Security.Claims.Claim(ClaimTypes.Role, user.Role),
                    new System.Security.Claims.Claim("UserId", user.Id.ToString())
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
