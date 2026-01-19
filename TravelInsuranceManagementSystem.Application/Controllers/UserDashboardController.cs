using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize(Roles = "User")]
    public class UserDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Claims()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("SignIn", "Account");

            int userId = int.Parse(userIdClaim.Value);

            var myClaims = await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.Policy.UserId == userId)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();

            return View(myClaims);
        }

        public IActionResult ClaimCreate()
        {
            return View("~/Views/UserDashboard/ClaimCreate.cshtml");
        }

        public async Task<IActionResult> Policies()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("SignIn", "Account");

            int userId = int.Parse(userIdClaim.Value);

            var myPolicies = await _context.Policies
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.TravelStartDate)
                .ToListAsync();

            return View(myPolicies);
        }

        // --- UPDATED RAISE TICKET (GET) ---
        [HttpGet]
        public IActionResult RaiseTicket()
        {
            // Pull the ID directly from the Claims you set in AccountController
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Session expired. Please re-login.";
                return RedirectToAction("SignIn", "Account");
            }

            // Pass the ID to the View (matches your @ViewBag.CurrentUserId)
            ViewBag.CurrentUserId = userId;

            return View("~/Views/UserDashboard/RaiseTicket.cshtml");
        }

        // --- UPDATED RAISE TICKET (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RaiseTicket(IFormCollection form)
        {
            // Securely get ID from Claims (Prevents form tampering)
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.Error = "User session not found. Please log in again.";
                return View("~/Views/UserDashboard/RaiseTicket.cshtml");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Create the CORE ticket 
                var coreTicket = new SupportTicket
                {
                    UserId = userId, // Using the ID directly from Claims
                    IssueDescription = form["Description"],
                    TicketStatus = "Open",
                    CreatedDate = DateTime.Now
                };

                _context.SupportTickets.Add(coreTicket);
                await _context.SaveChangesAsync();

                // 2. Create the TICKET DETAILS
                var ticketDetail = new TicketDetail
                {
                    TicketId = coreTicket.TicketId,
                    Subject = form["Subject"],
                    Category = form["Category"],
                    Priority = form["Priority"],
                    RelatedId = form["RelatedId"],
                    ContactMethod = form["ContactMethod"],
                    ContactDetail = form["ContactDetail"]
                };

                _context.TicketDetails.Add(ticketDetail);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Your ticket has been raised successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ViewBag.Error = "Database Error: " + (ex.InnerException?.Message ?? ex.Message);

                // Re-populate the ID if the view needs to be returned
                ViewBag.CurrentUserId = userId;
                return View("~/Views/UserDashboard/RaiseTicket.cshtml");
            }
        }
    }
}