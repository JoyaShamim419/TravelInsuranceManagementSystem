using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
namespace TravelInsuranceManagementSystem.Repo.Implementation
{
    public class AgentRepository : IAgentRepository
    {
        private readonly ApplicationDbContext _context;

        public AgentRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<Policy>> GetPoliciesWithMembersAsync() =>
            await _context.Policies.Include(p => p.Members).OrderByDescending(p => p.PolicyId).ToListAsync();

        public async Task<List<Claim>> GetClaimsWithCustomerAsync() =>
            await _context.Claims.Include(c => c.Policy).ThenInclude(p => p.User)
                .OrderByDescending(c => c.ClaimDate).ToListAsync();
        public async Task<List<SupportTicket>> GetSupportTicketsAsync() =>
            await _context.SupportTickets.OrderByDescending(t => t.TicketId).ToListAsync();

        public async Task<List<Payment>> GetPaymentsWithPolicyAsync() =>
            await _context.Payments.Include(p => p.Policy).OrderByDescending(p => p.PaymentDate).ToListAsync();

        public async Task<Claim> GetClaimByIdAsync(int id) => await _context.Claims.FindAsync(id);

        public async Task<SupportTicket> GetTicketByIdAsync(int id) => await _context.SupportTickets.FindAsync(id);

        public void UpdateClaim(Claim claim) => _context.Claims.Update(claim);

        public void UpdateTicket(SupportTicket ticket) => _context.SupportTickets.Update(ticket);

        public void DeleteTicket(SupportTicket ticket) => _context.SupportTickets.Remove(ticket);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
