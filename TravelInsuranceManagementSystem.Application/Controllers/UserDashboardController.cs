using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelInsuranceManagementSystem.Application.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
//using System.Linq

// IMPORTANT: This must match the namespace in your AccountController
namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize] // Only logged-in users can enter
    public class UserDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        // 👇 2. Add this constructor
        public UserDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            // This will look for Views/UserDashboard/Dashboard.cshtml
            return View();
        }

        public async Task<IActionResult> Claims()
        {
            // 1. Get the Current Logged-in User's ID
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            // 2. Fetch Claims from Database
            var myClaims = await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.Policy.UserId == userId)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();

            // 3. Send the data to the View
            return View(myClaims);
        }

        public IActionResult ClaimCreate()
        {
            return View("~/Views/UserDashboard/ClaimCreate.cshtml");
        }

        public async Task<IActionResult> Policies()
        {
            // 1. Get the Current Logged-in User's ID
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            // 2. Fetch Policies from Database for THIS user
            var myPolicies = await _context.Policies
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.TravelStartDate) // Show most recent trips first
                .ToListAsync();

            // 3. Send data to the View
            return View(myPolicies);
        }

        public IActionResult RaiseTicket()
        {
            return View("~/Views/UserDashboard/RaiseTicket.cshtml");
        }
    }
}