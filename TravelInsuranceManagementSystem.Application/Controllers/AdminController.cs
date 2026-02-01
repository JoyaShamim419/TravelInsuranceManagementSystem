using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models; // Assumed location based on Repo structureusing TravelInsuranceManagementSystem.Services.Interfaces;
namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly SignInManager<User> _signInManager;

        public AdminController(IAdminService adminService, SignInManager<User> signInManager) { _adminService = adminService; _signInManager = signInManager; }
        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }
        public async Task<IActionResult> Policies()
        {
            ViewData["Title"] = "Policy Management";
            var policies = await _adminService.GetPolicyManagementDataAsync();
            return View(policies);
        }
        public async Task<IActionResult> Payments()
        {
            ViewData["Title"] = "Payment History";
            var payments = await _adminService.GetPaymentHistoryAsync();
            return View(payments);
        }

        public async Task<IActionResult> Claims()
        {
            ViewData["Title"] = "Claims Overview";
            var claimsData = await _adminService.GetClaimsOverviewAsync();
            return View(claimsData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Account");
        }
    }
}
