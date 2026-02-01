using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models; // Adjust namespace if ClaimViewModel is elsewherenamespace TravelInsuranceManagementSystem.Services.Interfaces{
public interface IAdminService
{
    Task<List<Policy>> GetPolicyManagementDataAsync();
    Task<List<Payment>> GetPaymentHistoryAsync();
    Task<List<ClaimViewModel>> GetClaimsOverviewAsync();
}

 