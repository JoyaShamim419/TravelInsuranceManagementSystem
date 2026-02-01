using Microsoft.EntityFrameworkCore;

using TravelInsuranceManagementSystem.Repo.Data;

using TravelInsuranceManagementSystem.Repo.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

namespace TravelInsuranceManagementSystem.Repo.Implementation

{

    public class ClaimRepository : IClaimRepository

    {

        private readonly ApplicationDbContext _context;

        public ClaimRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<(bool Success, string Message)> SubmitClaimAsync(Claim claim, int userId, string uniqueFileName)

        {

            // 1. Logic: Verify Policy Ownership

            bool isOwner = await _context.Policies

                .AnyAsync(p => p.PolicyId == claim.PolicyId && p.UserId == userId);

            if (!isOwner)

            {

                return (false, "Invalid Policy ID. You can only file claims for your own policies.");

            }

            // 2. Logic: Set Default Values

            claim.Status = ClaimStatus.Pending;

            claim.ClaimDate = DateTime.Now;

            if (!string.IsNullOrEmpty(uniqueFileName))

            {

                claim.DocumentPath = uniqueFileName;

            }

            // 3. Logic: Save to Database

            await _context.Claims.AddAsync(claim);

            await _context.SaveChangesAsync();

            return (true, "Success");

        }

    }

}
