
using TravelInsuranceManagementSystem.Application.Models;
namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IClaimRepository
    {
        Task AddAsync(Claim claim);
        Task SaveAsync();
        bool ValidatePolicyOwnership(int policyId, int userId);
    }
}
