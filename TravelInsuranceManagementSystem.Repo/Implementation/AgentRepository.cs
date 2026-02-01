using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Data;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Repo.Implementation

{

    public class AgentRepository : IAgentRepository

    {

        private readonly ApplicationDbContext _context;

        public AgentRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<List<Policy>> GetPoliciesWithMembersAsync() =>

            await _context.Policies

                .Include(p => p.Members)

                .OrderByDescending(p => p.PolicyId)

                .ToListAsync();

        public async Task<List<Claim>> GetClaimsWithCustomerAsync() =>

            await _context.Claims

                .Include(c => c.Policy)

                .ThenInclude(p => p.User)

                .OrderByDescending(c => c.ClaimDate)

                .ToListAsync();

        public async Task<List<SupportTicket>> GetSupportTicketsAsync() =>

            await _context.SupportTickets

                .OrderByDescending(t => t.TicketId)

                .ToListAsync();

        public async Task<List<Payment>> GetPaymentsWithPolicyAsync() =>

            await _context.Payments

                .Include(p => p.Policy)

                .OrderByDescending(p => p.PaymentDate)

                .ToListAsync();

        // LOGIC MOVED HERE: Update Claim Status

        public async Task<(bool Success, string Message)> UpdateClaimStatusAsync(int id, string status, int agentId)

        {

            var claim = await _context.Claims.FindAsync(id);

            if (claim == null) return (false, "Claim not found");

            if (Enum.TryParse<ClaimStatus>(status, true, out var newStatus))

            {

                claim.Status = newStatus;

                claim.AgentId = agentId;

                _context.Claims.Update(claim);

                await _context.SaveChangesAsync();

                return (true, string.Empty);

            }

            return (false, "Invalid Status");

        }

        // LOGIC MOVED HERE: Update Ticket Status

        public async Task<bool> UpdateTicketStatusAsync(int id, string status)

        {

            var ticket = await _context.SupportTickets.FindAsync(id);

            if (ticket == null) return false;

            ticket.TicketStatus = status;

            _context.SupportTickets.Update(ticket);

            await _context.SaveChangesAsync();

            return true;

        }

        // LOGIC MOVED HERE: Delete Ticket

        public async Task<bool> DeleteTicketAsync(int id)

        {

            var ticket = await _context.SupportTickets.FindAsync(id);

            if (ticket == null) return false;

            _context.SupportTickets.Remove(ticket);

            await _context.SaveChangesAsync();

            return true;

        }

    }

}
