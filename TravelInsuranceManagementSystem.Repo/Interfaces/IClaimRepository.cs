using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IClaimRepository
    {
        Task<(bool Success, string Message)> SubmitClaimAsync(Claim claim, int userId, string uniqueFileName);
    }
}