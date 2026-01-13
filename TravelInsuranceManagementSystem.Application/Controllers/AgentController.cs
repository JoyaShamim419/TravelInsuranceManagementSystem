using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;

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

        // READ OPERATION: Fetch all tickets from the database
        public async Task<IActionResult> SupportTickets()
        {
            // Fetching tickets and ordering by most recent
            var tickets = await _context.SupportTickets
                .OrderByDescending(t => t.TicketId)
                .ToListAsync();
            return View(tickets);
        }

        // UPDATE OPERATION: AJAX endpoint to update status
        [HttpPost]
        public async Task<IActionResult> UpdateTicketStatus(int id, string status)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket == null) return NotFound();

            ticket.TicketStatus = status;
            // Optionally set the AgentId of the person who resolved it
            // ticket.AgentId = User.FindFirst("UserId")?.Value; 

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