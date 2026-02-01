using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepo;

        public PolicyService(IPolicyRepository policyRepo)
        {
            _policyRepo = policyRepo;
        }

        public async Task<int> CreateFamilyPolicyAsync(FamilyInsuranceDto data, int userId) =>
            await _policyRepo.CreateFamilyPolicyAsync(data, userId);
    }
}