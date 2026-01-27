using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
namespace TravelInsuranceManagementSystem.Repo.Implementation
{
    public class UserDashboardRepository : IUserDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public UserDashboardRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<Claim>> GetClaimsByUserIdAsync(int userId)
        {
            return await _context.Claims
                .Include(c => c.Policy).Where(c => c.Policy.UserId == userId).OrderByDescending(c => c.ClaimDate).ToListAsync();
        }
        public async Task<List<Policy>> GetPoliciesByUserIdAsync(int userId)
        {
            return await _context.Policies
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.TravelStartDate)
                .ToListAsync();
        }

        public async Task CreateTicketWithDetailsAsync(SupportTicket ticket, TicketDetail detail)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.SupportTickets.Add(ticket);
                await _context.SaveChangesAsync(); // Save to get the TicketId

                detail.TicketId = ticket.TicketId;
                _context.TicketDetails.Add(detail);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
