using Microsoft.EntityFrameworkCore;

using TravelInsuranceManagementSystem.Application.Data;

using InsuranceClaim = TravelInsuranceManagementSystem.Application.Models.Claim; // Resolve Ambiguity

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

        public async Task AddAsync(InsuranceClaim claim)

        {

            await _context.Claims.AddAsync(claim);

        }

        public async Task SaveAsync()

        {

            await _context.SaveChangesAsync();

        }

        public bool ValidatePolicyOwnership(int policyId, int userId)

        {

            return _context.Policies.Any(p => p.PolicyId == policyId && p.UserId == userId);

        }

    }

}
