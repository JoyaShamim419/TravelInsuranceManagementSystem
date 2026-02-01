using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models;    // Ensure ClaimViewModel is accessible here
using TravelInsuranceManagementSystem.Repo.Data; // Ensure this points to your DbContext namespace
using TravelInsuranceManagementSystem.Repo.Interfaces;

namespace TravelInsuranceManagementSystem.Repo.Implementation

{

    public class AdminRepository : IAdminRepository

    {

        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<List<Policy>> GetAllPoliciesWithMembersAsync() =>

            await _context.Policies

                .Include(p => p.Members)

                .OrderByDescending(p => p.PolicyId)

                .ToListAsync();

        public async Task<List<Payment>> GetAllPaymentsWithPoliciesAsync() =>

            await _context.Payments

                .Include(p => p.Policy)

                .OrderByDescending(p => p.PaymentDate)

                .ToListAsync();

        public async Task<List<ClaimViewModel>> GetClaimsOverviewAsync()

        {

            // Logic moved from Service to Repository

            return await (from c in _context.Claims

                          join p in _context.Policies on c.PolicyId equals p.PolicyId

                          join u in _context.Users on p.UserId equals u.Id

                          join a in _context.Users on c.AgentId equals a.Id into agentGroup

                          from agent in agentGroup.DefaultIfEmpty()

                          orderby c.ClaimDate descending

                          select new ClaimViewModel

                          {

                              ClaimId = c.ClaimId,

                              PolicyId = c.PolicyId,

                              CustomerId = u.Id,

                              CustomerName = u.FullName,

                              Plan = p.CoverageType,

                              CoverageType = c.IncidentType,

                              Amount = c.ClaimAmount,

                              ClaimDate = c.ClaimDate,

                              Status = c.Status.ToString(),

                              AgentName = agent != null ? agent.FullName : "Unassigned"

                          }).ToListAsync();

        }

    }

}
