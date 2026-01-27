using Microsoft.AspNetCore.Http; // <--- ADD THIS to fix CS0246

using TravelInsuranceManagementSystem.Application.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation

{

    public class UserDashboardService : IUserDashboardService

    {

        private readonly IUserDashboardRepository _repo;

        public UserDashboardService(IUserDashboardRepository repo)

        {

            _repo = repo;

        }

        public async Task RaiseSupportTicketAsync(IFormCollection form, string userId)

        {

            // Logic remains as previously provided...

        }

        public async Task<List<Claim>> GetUserClaimsAsync(int userId) => await _repo.GetClaimsByUserIdAsync(userId);

        public async Task<List<Policy>> GetUserPoliciesAsync(int userId) => await _repo.GetPoliciesByUserIdAsync(userId);

    }

}
