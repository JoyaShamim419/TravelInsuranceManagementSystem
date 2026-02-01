using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Services.Interfaces
{
    public interface IPolicyService
    {
        Task<int> CreateFamilyPolicyAsync(FamilyInsuranceDto data, int userId);
    }
}