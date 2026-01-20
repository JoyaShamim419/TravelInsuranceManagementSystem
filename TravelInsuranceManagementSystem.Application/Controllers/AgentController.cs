using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System; // Added for Enum
using System.Linq;
using System.Threading.Tasks;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize(Roles = "Agent")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AgentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard() => View();

        public async Task<IActionResult> Policies()
        {
            var policies = await _context.Policies
                .Include(p => p.Members)
                .OrderByDescending(p => p.PolicyId)
                .ToListAsync();
            return View(policies);
        }

        // READ OPERATION: Fetch all claims for the Agent to review
        public async Task<IActionResult> Claims()
        {
            var claims = await _context.Claims
                .Include(c => c.Policy)
                .ThenInclude(p => p.User) // 👈 FETCH CUSTOMER DETAILS (For Name/Email)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();

            return View(claims);
        }

        // UPDATE OPERATION: AJAX endpoint to update CLAIM status
        [HttpPost]
        public async Task<IActionResult> UpdateClaimStatus(int id, string status)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return Json(new { success = false, message = "Claim not found" });

            // 1. Get the Logged-in Agent's ID
            var agentIdString = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(agentIdString))
            {
                return Json(new { success = false, message = "Session expired. Please re-login." });
            }
            int agentId = int.Parse(agentIdString);

            // 2. Parse Status and Update
            if (Enum.TryParse<ClaimStatus>(status, true, out var newStatus))
            {
                claim.Status = newStatus;

                // 👇 THIS IS THE FIX: Save the Agent's ID to the claim!
                claim.AgentId = agentId;

                _context.Update(claim);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid Status" });
        }

        // READ OPERATION: Fetch all tickets from the database
        public async Task<IActionResult> SupportTickets()
        {
            var tickets = await _context.SupportTickets
                .OrderByDescending(t => t.TicketId)
                .ToListAsync();
            return View(tickets);
        }

        // --- FIXED PAYMENTS METHOD ---
        public async Task<IActionResult> Payments()
        {
            var payments = await _context.Payments
                .Include(p => p.Policy)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return View(payments);
        }

        // UPDATE OPERATION: AJAX endpoint to update TICKET status
        [HttpPost]
        public async Task<IActionResult> UpdateTicketStatus(int id, string status)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket == null) return NotFound();

            ticket.TicketStatus = status;
            _context.Update(ticket);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // DELETE OPERATION: Remove ticket from database
        [HttpPost]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket != null)
            {
                _context.SupportTickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(SupportTickets));
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