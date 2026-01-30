using Microsoft.AspNetCore.Http;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IUserDashboardRepository _repo;

        public UserDashboardService(IUserDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task RaiseSupportTicketAsync(IFormCollection form, string userId)
        {
            // 1. Map Main Ticket Data
            // Note: 'Description' comes from the HTML name attribute, 
            // but is saved to 'IssueDescription' in your DB model.
            var ticket = new SupportTicket
            {
                UserId = userId,
                IssueDescription = form["Description"],
                TicketStatus = "Open",
                CreatedDate = DateTime.Now
            };

            // 2. Map Extended Ticket Details
            var detail = new TicketDetail
            {
                Subject = form["Subject"],
                Category = form["Category"],
                Priority = form["Priority"],
                RelatedId = form["RelatedId"],
                ContactMethod = form["ContactMethod"],
                ContactDetail = form["ContactDetail"]
            };

            // 3. Optional: Handle File Upload logic here if you add a FilePath property to TicketDetail
            // var file = form.Files["Attachment"];

            // 4. Pass both objects to the repository
            await _repo.CreateTicketWithDetailsAsync(ticket, detail);
        }

        public async Task<List<Claim>> GetUserClaimsAsync(int userId) =>
            await _repo.GetClaimsByUserIdAsync(userId);

        public async Task<List<Policy>> GetUserPoliciesAsync(int userId) =>
            await _repo.GetPoliciesByUserIdAsync(userId);
    }
}