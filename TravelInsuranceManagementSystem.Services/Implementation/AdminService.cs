using TravelInsuranceManagementSystem.Repo.Models;

using TravelInsuranceManagementSystem.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation

{

    public class AdminService : IAdminService

    {

        private readonly IAdminRepository _adminRepo;

        public AdminService(IAdminRepository adminRepo)

        {

            _adminRepo = adminRepo;

        }

        public async Task<List<Policy>> GetPolicyManagementDataAsync() =>

            await _adminRepo.GetAllPoliciesWithMembersAsync();

        public async Task<List<Payment>> GetPaymentHistoryAsync() =>

            await _adminRepo.GetAllPaymentsWithPoliciesAsync();

        public async Task<List<ClaimViewModel>> GetClaimsOverviewAsync() =>

            await _adminRepo.GetClaimsOverviewAsync();

        // NEW

        public async Task<AdminDashboardViewModel> GetDashboardSummaryAsync() =>

            await _adminRepo.GetDashboardSummaryAsync();

    }

}
