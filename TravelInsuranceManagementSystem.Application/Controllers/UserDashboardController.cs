using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// IMPORTANT: This must match the namespace in your AccountController
namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize] // Only logged-in users can enter
    public class UserDashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            // This will look for Views/UserDashboard/Dashboard.cshtml
            return View();
        }

        public IActionResult Claims()
        {
            return View("~/Views/UserDashboard/Claims.cshtml");
        }

        public IActionResult ClaimCreate()
        {
            return View("~/Views/UserDashboard/ClaimCreate.cshtml");
        }

        public IActionResult Policies()
        {
            return View("~/Views/UserDashboard/Policies.cshtml");
        }

        public IActionResult RaiseTicket()
        {
            return View("~/Views/UserDashboard/RaiseTicket.cshtml");
        }
    }
}