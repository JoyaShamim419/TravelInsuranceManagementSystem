using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using TravelInsuranceManagementSystem.Application.Models;

using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Application.Controllers

{

    [Authorize(Roles = "Admin")]

    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

    public class AdminController : Controller

    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)

        {

            _adminService = adminService;

        }

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

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("SignIn", "Home");

        }

    }

}
