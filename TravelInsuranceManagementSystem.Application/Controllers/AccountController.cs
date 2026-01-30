using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Services.Interfaces;
using System.Security.Claims;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(IAccountService accountService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult SignIn() => View("~/Views/Home/SignIn.cshtml");

        [HttpPost]
        public async Task<IActionResult> SignIn(string Email, string Password)
        {
            // 1. Fetch user to verify they exist and get their custom Role property
            var user = await _accountService.GetUserByEmail(Email);

            if (user != null)
            {
                // 2. Initial password check
                var result = await _signInManager.PasswordSignInAsync(user, Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // 3. FIX: Use fully qualified name 'System.Security.Claims.Claim' 
                    // This resolves the error where it confuses your Model 'Claim' with Security 'Claim'
                    var claims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role),
                        new System.Security.Claims.Claim("UserId", user.Id.ToString()),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Email)
                    };

                    // 4. Sign in again with the specific Role and UserId claims attached
                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

                    // 5. Redirect based on the role found in DB
                    return user.Role switch
                    {
                        "Admin" => RedirectToAction("Dashboard", "Admin"),
                        "Agent" => RedirectToAction("Dashboard", "Agent"),
                        _ => RedirectToAction("Dashboard", "UserDashboard")
                    };
                }
            }

            TempData["ErrorMessage"] = "Invalid email or password!";
            return View("~/Views/Home/SignIn.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            // RegisterUser handles hashing the password via Identity UserManager
            var result = await _accountService.RegisterUser(user, user.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful! Please sign in.";
                return RedirectToAction("SignIn");
            }

            // Combine errors into a single string for the view
            TempData["ErrorMessage"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return View("~/Views/Home/SignIn.cshtml");
        }

        // FIX: This method prevents the 404 error when a user is unauthorized
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _accountService.GetUserByEmail(email);
            if (user == null)
            {
                ViewBag.Error = "Email address not found.";
                return View();
            }

            var token = await _accountService.GeneratePasswordResetToken(user);
            return RedirectToAction("ResetPassword", new { email = email, token = token });
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string token, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            var user = await _accountService.GetUserByEmail(email);
            if (user != null)
            {
                var result = await _accountService.ResetPassword(user, token, newPassword);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Password reset successfully!";
                    return RedirectToAction("SignIn");
                }
                ViewBag.Error = string.Join(" ", result.Errors.Select(e => e.Description));
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}