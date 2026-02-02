using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Services.Interfaces;

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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult SignIn(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            // Pass the return URL to the View so it can be used in the form
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Home/SignIn.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string Email, string Password, string returnUrl = null)
        {
            var user = await _accountService.GetUserByEmail(Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var claims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role),
                        new System.Security.Claims.Claim("UserId", user.Id.ToString()),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Email)
                    };

                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

                    // --- REDIRECTION LOGIC ---
                    // Case A: If user came from "Insurance" page, send them back there
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }

                    // Case B: Default Dashboard Redirection
                    return user.Role switch
                    {
                        "Admin" => RedirectToAction("Dashboard", "Admin"),
                        "Agent" => RedirectToAction("Dashboard", "Agent"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }
            }

            TempData["ErrorMessage"] = "Invalid email or password!";
            // Ensure ReturnUrl is preserved if login fails
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Home/SignIn.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            var result = await _accountService.RegisterUser(user, user.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful! Please sign in.";
                return RedirectToAction("SignIn");
            }
            TempData["ErrorMessage"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return View("~/Views/Home/SignIn.cshtml");
        }

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