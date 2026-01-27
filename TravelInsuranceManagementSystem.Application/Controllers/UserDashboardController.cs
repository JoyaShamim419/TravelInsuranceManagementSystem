using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelInsuranceManagementSystem.Services.Interfaces;
namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize(Roles = "User")]
    public class UserDashboardController : Controller
    {
        private readonly IUserDashboardService _dashboardService;

        public UserDashboardController(IUserDashboardService dashboardService) { _dashboardService = dashboardService; }
        public IActionResult Dashboard() => View();

        public async Task<IActionResult> Claims()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return RedirectToAction("SignIn", "Account");

            var myClaims = await _dashboardService.GetUserClaimsAsync(int.Parse(userId));
            return View(myClaims);
        }
        public IActionResult ClaimCreate() => View("~/Views/UserDashboard/ClaimCreate.cshtml");

        public async Task<IActionResult> Policies()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return RedirectToAction("SignIn", "Account");

            var myPolicies = await _dashboardService.GetUserPoliciesAsync(int.Parse(userId));
            return View(myPolicies);
        }
        [HttpGet]
        public IActionResult RaiseTicket()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("SignIn", "Account");

            ViewBag.CurrentUserId = userId; return View("~/Views/UserDashboard/RaiseTicket.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RaiseTicket(IFormCollection form)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("SignIn", "Account");

            try
            {
                await _dashboardService.RaiseSupportTicketAsync(form, userId);
                TempData["SuccessMessage"] = "Your ticket has been raised successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Database Error: " + (ex.InnerException?.Message ?? ex.Message);
                ViewBag.CurrentUserId = userId;
                return View("~/Views/UserDashboard/RaiseTicket.cshtml");
            }
        }
    }
}
