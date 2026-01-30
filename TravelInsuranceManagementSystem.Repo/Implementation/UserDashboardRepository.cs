using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;

namespace TravelInsuranceManagementSystem.Repo.Implementation
{
    public class UserDashboardRepository : IUserDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public UserDashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Claim>> GetClaimsByUserIdAsync(int userId)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.Policy.UserId == userId)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();
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
            // Use a transaction because we are modifying two related tables
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Step 1: Save the SupportTicket to generate the TicketId (Identity Column)
                _context.SupportTickets.Add(ticket);
                await _context.SaveChangesAsync();

                // Step 2: Assign the newly generated ID to the detail record
                detail.TicketId = ticket.TicketId;

                // Step 3: Save the details
                _context.TicketDetails.Add(detail);
                await _context.SaveChangesAsync();

                // Step 4: Finalize the transaction
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // If any part fails, roll back the database to its previous state
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}