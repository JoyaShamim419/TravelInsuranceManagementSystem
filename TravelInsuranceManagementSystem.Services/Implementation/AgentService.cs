using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepo;

        public AgentService(IAgentRepository agentRepo)
        {
            _agentRepo = agentRepo;
        }

        public async Task<List<Policy>> GetPoliciesAsync() =>
            await _agentRepo.GetPoliciesWithMembersAsync();

        // FIXED: Full namespace prevents error CS0104 (Ambiguity)
        public async Task<List<TravelInsuranceManagementSystem.Application.Models.Claim>> GetClaimsAsync() =>
            await _agentRepo.GetClaimsWithCustomerAsync();

        public async Task<List<SupportTicket>> GetSupportTicketsAsync() =>
            await _agentRepo.GetSupportTicketsAsync();

        public async Task<List<Payment>> GetPaymentsAsync() =>
            await _agentRepo.GetPaymentsWithPolicyAsync();

        public async Task<(bool Success, string Message)> UpdateClaimStatusAsync(int id, string status, int agentId)
        {
            var claim = await _agentRepo.GetClaimByIdAsync(id);
            if (claim == null) return (false, "Claim not found");

            if (Enum.TryParse<ClaimStatus>(status, true, out var newStatus))
            {
                claim.Status = newStatus;
                claim.AgentId = agentId;
                _agentRepo.UpdateClaim(claim);
                await _agentRepo.SaveAsync();
                return (true, string.Empty);
            }
            return (false, "Invalid Status");
        }

        public async Task<bool> UpdateTicketStatusAsync(int id, string status)
        {
            var ticket = await _agentRepo.GetTicketByIdAsync(id);
            if (ticket == null) return false;

            ticket.TicketStatus = status;
            _agentRepo.UpdateTicket(ticket);
            await _agentRepo.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var ticket = await _agentRepo.GetTicketByIdAsync(id);
            if (ticket == null) return false;

            _agentRepo.DeleteTicket(ticket);
            await _agentRepo.SaveAsync();
            return true;
        }
    }
}