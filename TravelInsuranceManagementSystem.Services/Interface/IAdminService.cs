using TravelInsuranceManagementSystem.Repo.Models;

using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Services.Interfaces

{

    public interface IAdminService

    {

        Task<List<Policy>> GetPolicyManagementDataAsync();

        Task<List<Payment>> GetPaymentHistoryAsync();

        Task<List<ClaimViewModel>> GetClaimsOverviewAsync();

        // NEW

        Task<AdminDashboardViewModel> GetDashboardSummaryAsync();

    }

}
