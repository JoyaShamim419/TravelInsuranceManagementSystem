using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models; // If needed for legacy models, otherwise remove

namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IAgentRepository
    {
        // Existing read methods
        Task<List<Policy>> GetPoliciesWithMembersAsync();
        Task<List<Claim>> GetClaimsWithCustomerAsync();
        Task<List<SupportTicket>> GetSupportTicketsAsync();
        Task<List<Payment>> GetPaymentsWithPolicyAsync();
        // --- NEW METHODS YOU ARE MISSING ---
        // These must be here for the Service to find them
        Task<(bool Success, string Message)> UpdateClaimStatusAsync(int id, string status, int agentId);
        Task<bool> UpdateTicketStatusAsync(int id, string status);
        Task<bool> DeleteTicketAsync(int id);
    }
}