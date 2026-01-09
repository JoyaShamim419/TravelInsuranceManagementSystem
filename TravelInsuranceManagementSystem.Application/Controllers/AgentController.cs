using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    // Restrict access to users with the 'Agent' role only
    [Authorize(Roles = "Agent")]
    // Prevent the browser from caching these private pages
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AgentController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Policies()
        {
            return View();
        }

        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult Payments()
        {
            return View();
        }

        public IActionResult SupportTickets()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Clear the authentication cookie explicitly using the Cookie Scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear session data if any exists
            HttpContext.Session.Clear();

            // Redirect to the Home/SignIn page after logout
            return RedirectToAction("SignIn", "Home");
        }
    }
}