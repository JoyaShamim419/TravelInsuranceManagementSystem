using Microsoft.EntityFrameworkCore;

using TravelInsuranceManagementSystem.Application.Data;

using TravelInsuranceManagementSystem.Application.Models;

using TravelInsuranceManagementSystem.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

namespace TravelInsuranceManagementSystem.Repo.Implementation

{

    public class AdminRepository : IAdminRepository

    {

        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        public async Task<List<Policy>> GetAllPoliciesWithMembersAsync() =>

            await _context.Policies.Include(p => p.Members).OrderByDescending(p => p.PolicyId).ToListAsync();

        public async Task<List<Payment>> GetAllPaymentsWithPoliciesAsync() =>

            await _context.Payments.Include(p => p.Policy).OrderByDescending(p => p.PaymentDate).ToListAsync();

        public IQueryable<User> GetUsersQuery() => _context.Users.AsQueryable();

        public IQueryable<Policy> GetPoliciesQuery() => _context.Policies.AsQueryable();

        public IQueryable<Claim> GetClaimsQuery() => _context.Claims.AsQueryable();

        public async Task<List<Claim>> GetAllClaimsRawAsync() => await _context.Claims.ToListAsync();

    }

}
