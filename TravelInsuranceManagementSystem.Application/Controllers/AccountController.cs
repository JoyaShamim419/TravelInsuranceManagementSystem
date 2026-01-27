using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TravelInsuranceManagementSystem.Models; // Adjust to your Model namespace

using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Application.Controllers

{

    public class AccountController : Controller

    {

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)

        {

            _accountService = accountService;

        }

        [HttpGet]

        public IActionResult SignIn() => View("~/Views/Home/SignIn.cshtml");

        [HttpPost]

        public async Task<IActionResult> SignIn(string Email, string Password)

        {

            var user = _accountService.Authenticate(Email, Password);

            if (user != null)

            {

                // 1. Generate JWT for Session/API

                var token = _accountService.GenerateJwtToken(user);

                // 2. Prepare Claims

                var authClaims = new List<Claim>

                {

                    new Claim(ClaimTypes.Name, user.FullName),

                    new Claim(ClaimTypes.Email, user.Email),

                    new Claim(ClaimTypes.Role, user.Role),

                    new Claim("UserId", user.Id.ToString())

                };

                // 3. Handle JSON/API Requests

                if (Request.Headers["Accept"].ToString().Contains("application/json"))

                {

                    return Ok(new { token = token, user = user.FullName, role = user.Role });

                }

                // 4. Handle Cookie Authentication

                var claimsIdentity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // 5. Store Token in Session

                HttpContext.Session.SetString("JWToken", token);

                // 6. Role-based Redirection

                return user.Role switch

                {

                    "Admin" => RedirectToAction("Dashboard", "Admin"),

                    "Agent" => RedirectToAction("Dashboard", "Agent"),

                    _ => RedirectToAction("Dashboard", "UserDashboard")

                };

            }

            // Handle Unauthorized for JSON

            if (Request.Headers["Accept"].ToString().Contains("application/json"))

                return Unauthorized(new { message = "Invalid email or password" });

            TempData["ErrorMessage"] = "Invalid email or password!";

            return View("~/Views/Home/SignIn.cshtml");

        }

        [HttpPost]

        public IActionResult SignUp(Models.User user)

        {

            bool isRegistered = _accountService.RegisterUser(user);

            if (isRegistered)

            {

                return RedirectToAction("SignIn");

            }

            TempData["ErrorMessage"] = "This email is already registered!";

            return View("~/Views/Home/SignIn.cshtml");

        }

        [HttpGet]

        public IActionResult ForgotPassword() => View();

        [HttpPost]

        public IActionResult ForgotPassword(string email)

        {

            var user = _accountService.GetUserByEmail(email);

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

            bool success = _accountService.ResetPassword(email, newPassword);

            if (success)

            {

                TempData["SuccessMessage"] = "Password reset successfully!";

                return RedirectToAction("SignIn");

            }

            return BadRequest();

        }

        public async Task<IActionResult> Logout()

        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");

        }

    }

}
