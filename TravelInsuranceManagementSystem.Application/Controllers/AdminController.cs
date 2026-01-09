using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    // This attribute prevents the "Back" button from showing admin data after logout
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }

        public IActionResult Policies()
        {
            ViewData["Title"] = "Policy Management";
            return View();
        }

        public IActionResult Claims()
        {
            ViewData["Title"] = "Claims Overview";
            return View();
        }

        public IActionResult Payments()
        {
            ViewData["Title"] = "Payment History";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Clear the server-side authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear any session-specific data
            HttpContext.Session.Clear();

            // Redirect back to the public Sign-In page
            return RedirectToAction("SignIn", "Home");
        }
    }
}