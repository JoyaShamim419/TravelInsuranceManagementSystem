using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Needed for sorting (OrderByDescending)
using System.Threading.Tasks; // Needed for async/await
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
// Ensure this namespace matches where your Payment and Policy models are
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor Injection
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }

        // READ OPERATION: Fetch all policies including family members
        public async Task<IActionResult> Policies()
        {
            ViewData["Title"] = "Policy Management";

            var policies = await _context.Policies
                .Include(p => p.Members) // Load family members
                .OrderByDescending(p => p.PolicyId) // Newest policies first
                .ToListAsync();

            return View(policies);
        }

        // READ OPERATION: Fetch all payments (Updated)
        public async Task<IActionResult> Payments()
        {
            ViewData["Title"] = "Payment History";

            // Fetch payments and include the related Policy data
            var payments = await _context.Payments
                .Include(p => p.Policy) // Join with Policy table to get User/Plan info
                .OrderByDescending(p => p.PaymentDate) // Newest payments first
                .ToListAsync();

            return View(payments);
        }

        public IActionResult Claims()
        {
            ViewData["Title"] = "Claims Overview";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear session
            HttpContext.Session.Clear();

            // Redirect to Login page
            return RedirectToAction("SignIn", "Home");
        }
    }
}