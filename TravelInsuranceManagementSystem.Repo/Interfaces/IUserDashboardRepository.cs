using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;
namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IUserDashboardRepository
    {
        Task<List<Claim>> GetClaimsByUserIdAsync(int userId);
        Task<List<Policy>> GetPoliciesByUserIdAsync(int userId);
        Task CreateTicketWithDetailsAsync(SupportTicket ticket, TicketDetail detail);
    }
}
