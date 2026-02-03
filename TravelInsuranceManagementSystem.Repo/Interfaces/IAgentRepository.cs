using TravelInsuranceManagementSystem.Repo.Models;

using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Repo.Interfaces

{

    public interface IAgentRepository

    {

        // 1. Policies

        Task<List<Policy>> GetPoliciesWithMembersAsync();

        // 2. Claims

        Task<List<Claim>> GetClaimsWithCustomerAsync();

        // 3. Support Tickets

        Task<List<SupportTicket>> GetSupportTicketsAsync();

        // 4. Payments

        Task<List<Payment>> GetPaymentsWithPolicyAsync();

        // 5. Update Actions

        Task<(bool Success, string Message)> UpdateClaimStatusAsync(int id, string status, int agentId);

        Task<bool> UpdateTicketStatusAsync(int id, string status);

        Task<bool> DeleteTicketAsync(int id);

        // 6. Dashboard Summary

        Task<AgentDashboardViewModel> GetDashboardSummaryAsync();

    }

}
