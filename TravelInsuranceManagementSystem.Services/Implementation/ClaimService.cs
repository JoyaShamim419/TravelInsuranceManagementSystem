using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepo;

        public ClaimService(IClaimRepository claimRepo)
        {
            _claimRepo = claimRepo;
        }

        public async Task<(bool Success, string Message)> SubmitClaimAsync(Claim claim, int userId, string uniqueFileName) =>
            await _claimRepo.SubmitClaimAsync(claim, userId, uniqueFileName);
    }
}