using Microsoft.AspNetCore.Http;
using TravelInsuranceManagementSystem.Repo.Models;
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

        public async Task<List<Claim>> GetUserClaimsAsync(int userId) =>
            await _repo.GetClaimsByUserIdAsync(userId);

        public async Task<List<Policy>> GetUserPoliciesAsync(int userId) =>
            await _repo.GetPoliciesByUserIdAsync(userId);

        public async Task RaiseSupportTicketAsync(IFormCollection form, int userId) =>
            await _repo.RaiseSupportTicketAsync(form, userId);

        public async Task<UserDashboardViewModel> GetDashboardSummaryAsync(int userId) =>
            await _repo.GetDashboardSummaryAsync(userId);
    }
}