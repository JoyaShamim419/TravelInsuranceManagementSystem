using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Services.Interfaces

{

    public interface IClaimService

    {

        Task<(bool Success, string Message)> SubmitClaimAsync(Claim claim, int userId, string uniqueFileName);

    }

}
