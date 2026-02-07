using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

using TravelInsuranceManagementSystem.Models;

using TravelInsuranceManagementSystem.Repo.Models;

using TravelInsuranceManagementSystem.Models.ViewModels;

using TravelInsuranceManagementSystem.Repo.Data;

using TravelInsuranceManagementSystem.Repo.Interfaces;

using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation

{

    public class AdminService : IAdminService

    {

        private readonly IAdminRepository _adminRepo;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<User> _userManager;

        public AdminService(

            IAdminRepository adminRepo,

            ApplicationDbContext context,

            UserManager<User> userManager)

        {

            _adminRepo = adminRepo;

            _context = context;

            _userManager = userManager;

        }


        // EXISTING REPOSITORY METHODS (Wrapped)

       

        public async Task<List<Policy>> GetPolicyManagementDataAsync()

        {

            // Delegates to Repository to fetch all policies with user details

            return await _adminRepo.GetAllPoliciesWithMembersAsync();

        }

        public async Task<List<Payment>> GetPaymentHistoryAsync()

        {

            // Delegates to Repository to fetch payment history

            // Fixed Error: 'Payment' is now recognized due to namespaces above

            return await _adminRepo.GetAllPaymentsWithPoliciesAsync();

        }

        public async Task<List<ClaimViewModel>> GetClaimsOverviewAsync()

        {

            // Delegates to Repository to fetch claims data for the admin grid

            return await _adminRepo.GetClaimsOverviewAsync();

        }

        public async Task<AdminDashboardViewModel> GetDashboardSummaryAsync()

        {

            // Delegates to Repository to fetch the main dashboard stats/charts

            return await _adminRepo.GetDashboardSummaryAsync();

        }


      

        // NEW AGENT MANAGEMENT METHODS (Logic Implemented Here)

      

        public async Task<List<AgentWorkloadViewModel>> GetAgentsWithWorkloadAsync()

        {

            // 1. Fetch all users who have the role "Agent"

            var agents = await _userManager.GetUsersInRoleAsync("Agent");

            var agentList = new List<AgentWorkloadViewModel>();

            // 2. Loop through each agent to calculate their specific workload

            foreach (var user in agents)

            {

                // Count Claims handled by this agent

                // We check if the Claim.AgentId matches the current User.Id

                var claimCount = await _context.Claims

                    .CountAsync(c => c.AgentId == user.Id);

                
                // Create the View Model for the card

                var agentViewModel = new AgentWorkloadViewModel

                {

                    Id = user.Id,

                    FullName = user.FullName,

                    Email = user.Email,

                    Role = "Agent",

                    // Assign calculated counts

                    ClaimsHandled = claimCount,

                   

                   

                    // Assuming all agents fetched are active

                    IsActive = true

                };

                agentList.Add(agentViewModel);

            }

            return agentList;

        }

        public async Task<bool> DeleteAgentAsync(int userId)

        {

            // 1. Find the user by ID

            // We convert int to string because Identity FindByIdAsync expects a string

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)

            {

                // User not found, cannot delete

                return false;

            }

            // 2. Delete the user securely using Identity Manager

            // This ensures roles and claims are also cleaned up

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;

        }

    }

}
