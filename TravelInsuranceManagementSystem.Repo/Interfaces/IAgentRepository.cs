using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;
namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IAgentRepository
    {
        Task<List<Policy>> GetPoliciesWithMembersAsync(); Task<List<Claim>> GetClaimsWithCustomerAsync(); Task<List<SupportTicket>> GetSupportTicketsAsync(); Task<List<Payment>> GetPaymentsWithPolicyAsync(); Task<Claim> GetClaimByIdAsync(int id);
        Task<SupportTicket> GetTicketByIdAsync(int id);
        void UpdateClaim(Claim claim);
        void UpdateTicket(SupportTicket ticket);
        void DeleteTicket(SupportTicket ticket);
        Task SaveAsync();
    }
}
