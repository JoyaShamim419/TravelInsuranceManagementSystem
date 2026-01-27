using TravelInsuranceManagementSystem.Application.Data;

using TravelInsuranceManagementSystem.Application.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

namespace TravelInsuranceManagementSystem.Repo.Implementation

{

    public class PolicyRepository : IPolicyRepository

    {

        private readonly ApplicationDbContext _context;

        public PolicyRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        // Implementation of GetById to resolve CS1061

        public Policy GetById(int id)

        {

            return _context.Policies.FirstOrDefault(p => p.PolicyId == id);

        }

        public async Task AddPolicyAsync(Policy policy) =>

            await _context.Policies.AddAsync(policy);

        public async Task SaveAsync() =>

            await _context.SaveChangesAsync();

    }

}
