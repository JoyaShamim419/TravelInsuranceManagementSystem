using TravelInsuranceManagementSystem.Application.Models;

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

        public async Task<(bool Success, string Message)> SubmitClaimAsync(Claim claim, int userId, string uniqueFileName)

        {

            // Verify Ownership

            bool isOwner = _claimRepo.ValidatePolicyOwnership(claim.PolicyId, userId);

            if (!isOwner)

            {

                return (false, "Invalid Policy ID. You can only file claims for your own policies.");

            }

            // Set default values for the database model

            claim.Status = ClaimStatus.Pending;

            claim.ClaimDate = DateTime.Now;

            if (!string.IsNullOrEmpty(uniqueFileName))

            {

                claim.DocumentPath = uniqueFileName;

            }

            await _claimRepo.AddAsync(claim);

            await _claimRepo.SaveAsync();

            return (true, "Success");

        }

    }

}
