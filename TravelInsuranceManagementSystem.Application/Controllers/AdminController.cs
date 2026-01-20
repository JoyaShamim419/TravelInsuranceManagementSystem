using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }

        public async Task<IActionResult> Policies()
        {
            ViewData["Title"] = "Policy Management";
            var policies = await _context.Policies
                .Include(p => p.Members)
                .OrderByDescending(p => p.PolicyId)
                .ToListAsync();
            return View(policies);
        }

        public async Task<IActionResult> Payments()
        {
            ViewData["Title"] = "Payment History";
            var payments = await _context.Payments
                .Include(p => p.Policy)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
            return View(payments);
        }

        // 👇 UPDATED: Fetch Claims + Customer + Agent Name
        public async Task<IActionResult> Claims()
        {
            ViewData["Title"] = "Claims Overview";

            // LINQ Query to join tables and flatten the data
            var claimsData = await (from c in _context.Claims
                                    join p in _context.Policies on c.PolicyId equals p.PolicyId
                                    join u in _context.Users on p.UserId equals u.Id // The Customer

                                    // LEFT JOIN to find the Agent (because AgentId might be 0 or null)
                                    join a in _context.Users on c.AgentId equals a.Id into agentGroup
                                    from agent in agentGroup.DefaultIfEmpty()

                                    orderby c.ClaimDate descending
                                    select new ClaimViewModel
                                    {
                                        ClaimId = c.ClaimId,
                                        PolicyId = c.PolicyId,
                                        CustomerId = u.Id,
                                        CustomerName = u.FullName,
                                        Plan = p.CoverageType, // e.g. Gold/Platinum
                                        CoverageType = c.IncidentType, // e.g. Medical/Lost Baggage
                                        Amount = c.ClaimAmount,
                                        ClaimDate = c.ClaimDate,
                                        Status = c.Status.ToString(),
                                        // If agent is found, use their name. If not, check if ID > 0.
                                        AgentName = (agent != null) ? agent.FullName : "Unassigned"
                                    }).ToListAsync();

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

    // 👇 Helper Class to hold the combined data for the View
    public class ClaimViewModel
    {
        public int ClaimId { get; set; }
        public int PolicyId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Plan { get; set; }
        public string CoverageType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ClaimDate { get; set; }
        public string Status { get; set; }
        public string AgentName { get; set; }
    }
}