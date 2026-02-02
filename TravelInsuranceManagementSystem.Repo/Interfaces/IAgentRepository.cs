using TravelInsuranceManagementSystem.Repo.Models;

using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Repo.Interfaces

{

    public interface IAgentRepository

    {

        Task<List<Policy>> GetPoliciesWithMembersAsync();

        Task<List<Claim>> GetClaimsWithCustomerAsync();

        Task<List<SupportTicket>> GetSupportTicketsAsync();

        Task<List<Payment>> GetPaymentsWithPolicyAsync();

        Task<(bool Success, string Message)> UpdateClaimStatusAsync(int id, string status, int agentId);

        Task<bool> UpdateTicketStatusAsync(int id, string status);

        Task<bool> DeleteTicketAsync(int id);

        // --- NEW METHOD ---

        Task<AgentDashboardViewModel> GetDashboardSummaryAsync();

    }

}
