using Microsoft.AspNetCore.Http; // Needed for IFormCollection

using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Repo.Interfaces

{

    public interface IUserDashboardRepository

    {

        Task<List<Claim>> GetClaimsByUserIdAsync(int userId);

        Task<List<Policy>> GetPoliciesByUserIdAsync(int userId);

        Task RaiseSupportTicketAsync(IFormCollection form, int userId);

    }

}
