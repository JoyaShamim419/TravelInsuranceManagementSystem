using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<Policy>> GetAllPoliciesWithMembersAsync();
        Task<List<Payment>> GetAllPaymentsWithPoliciesAsync();
        Task<List<Claim>> GetAllClaimsRawAsync(); // For raw access if needed
        IQueryable<User> GetUsersQuery(); // For joining in the service
        IQueryable<Policy> GetPoliciesQuery();
        IQueryable<Claim> GetClaimsQuery();
    }
}