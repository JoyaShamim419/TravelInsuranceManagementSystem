using Microsoft.EntityFrameworkCore;

using TravelInsuranceManagementSystem.Application.Models;

using TravelInsuranceManagementSystem.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation

{

    public class AdminService : IAdminService

    {

        private readonly IAdminRepository _adminRepo;

        public AdminService(IAdminRepository adminRepo)

        {

            _adminRepo = adminRepo;

        }

        public async Task<List<Policy>> GetPolicyManagementDataAsync() =>

            await _adminRepo.GetAllPoliciesWithMembersAsync();

        public async Task<List<Payment>> GetPaymentHistoryAsync() =>

            await _adminRepo.GetAllPaymentsWithPoliciesAsync();

        public async Task<List<ClaimViewModel>> GetClaimsOverviewAsync()

        {

            var claims = _adminRepo.GetClaimsQuery();

            var policies = _adminRepo.GetPoliciesQuery();

            var users = _adminRepo.GetUsersQuery();

            return await (from c in claims

                          join p in policies on c.PolicyId equals p.PolicyId

                          join u in users on p.UserId equals u.Id

                          join a in users on c.AgentId equals a.Id into agentGroup

                          from agent in agentGroup.DefaultIfEmpty()

                          orderby c.ClaimDate descending

                          select new ClaimViewModel

                          {

                              ClaimId = c.ClaimId,

                              PolicyId = c.PolicyId,

                              CustomerId = u.Id,

                              CustomerName = u.FullName,

                              Plan = p.CoverageType,

                              CoverageType = c.IncidentType,

                              Amount = c.ClaimAmount,

                              ClaimDate = c.ClaimDate,

                              Status = c.Status.ToString(),

                              AgentName = agent != null ? agent.FullName : "Unassigned"

                          }).ToListAsync();

        }

    }

}
