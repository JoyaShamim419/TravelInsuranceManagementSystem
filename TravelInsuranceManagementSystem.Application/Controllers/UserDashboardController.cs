using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize]
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

        [HttpGet]
        public IActionResult RaiseTicket()
        {
            // 1. Try to get email from Name claim
            var email = User.Identity.Name;

            // 2. Fallback: If Name is null, try to find the NameIdentifier claim
            if (string.IsNullOrEmpty(email))
            {
                email = User.FindFirstValue(ClaimTypes.Name);
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                // This will help you debug if the session is alive but the lookup fails
                ViewBag.Error = $"System error: Could not find a user record for '{email}'. Please re-login.";
                return View("~/Views/UserDashboard/RaiseTicket.cshtml");
            }

            ViewBag.CurrentUserId = user.Id;
            return View("~/Views/UserDashboard/RaiseTicket.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RaiseTicket(IFormCollection form)
        {
            var currentUserName = User.Identity.Name;
            var userRecord = _context.Users.FirstOrDefault(u => u.Email == currentUserName);

            if (userRecord == null)
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
                    // Use userRecord.Id (from your User model)
                    UserId = userRecord.Id.ToString(),
                    IssueDescription = form["Description"],
                    TicketStatus = "Open",
                    CreatedDate = DateTime.Now
                };

                _context.SupportTickets.Add(coreTicket);
                await _context.SaveChangesAsync(); // Generates the TicketId

                // 2. Create the TICKET DETAILS (the extra table)
                var ticketDetail = new TicketDetail
                {
                    TicketId = coreTicket.TicketId, // FK to the core ticket
                    Subject = form["Subject"],
                    Category = form["Category"],
                    Priority = form["Priority"],
                    RelatedId = form["RelatedId"],
                    ContactMethod = form["ContactMethod"],
                    ContactDetail = form["ContactDetail"]
                };

                _context.TicketDetails.Add(ticketDetail);
                await _context.SaveChangesAsync();

                // 3. Commit the transaction
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Your ticket has been raised successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // inner exception provides the most helpful error details
                ViewBag.Error = "Database Error: " + (ex.InnerException?.Message ?? ex.Message);
                return View("~/Views/UserDashboard/RaiseTicket.cshtml");
            }
        }
    }
}