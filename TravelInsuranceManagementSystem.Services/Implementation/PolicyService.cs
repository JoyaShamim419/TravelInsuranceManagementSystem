using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Services.Interfaces;
namespace TravelInsuranceManagementSystem.Services.Implementation
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepo;

        public PolicyService(IPolicyRepository policyRepo) { _policyRepo = policyRepo; }
        public async Task<int> CreateFamilyPolicyAsync(FamilyInsuranceDto data, int userId)
        {            // Business Logic: Premium Calculationdecimal coverage = data.PolicyDetails.PlanType == "Premium" ? 50000 : 10000;

            var newPolicy = new Policy
            {
                UserId = userId, // Ensure we link the policy to the logged-in user
                DestinationCountry = data.PolicyDetails.Destination,
                TravelStartDate = data.PolicyDetails.TripStart,
                TravelEndDate = data.PolicyDetails.TripEnd,
                CoverageType = data.PolicyDetails.PlanType,
                PolicyStatus = PolicyStatus.ACTIVE,
               // CoverageAmount = coverage,
                Members = data.Members.Select(m => new PolicyMember
                {
                    Title = m.Title,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Relation = m.Relation,
                    DOB = m.DOB,
                    Mobile = m.Mobile
                }).ToList()
            };

            await _policyRepo.AddPolicyAsync(newPolicy);
            await _policyRepo.SaveAsync();

            return newPolicy.PolicyId;
        }
    }
}
