using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;

using TravelInsuranceManagementSystem.Repo.Data;

using TravelInsuranceManagementSystem.Repo.Models;

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

        // LOGIC MOVED HERE: Mapping Form Data & Transaction

        public async Task RaiseSupportTicketAsync(IFormCollection form, int userId)

        {

            // 1. Logic: Map Form to Entities

            var ticket = new SupportTicket

            {

                UserId = userId.ToString(), // Storing as string based on your model definition

                IssueDescription = form["Description"],

                TicketStatus = "Open",

                CreatedDate = DateTime.Now

            };

            var detail = new TicketDetail

            {

                Subject = form["Subject"],

                Category = form["Category"],

                Priority = form["Priority"],

                RelatedId = form["RelatedId"],

                ContactMethod = form["ContactMethod"],

                ContactDetail = form["ContactDetail"]

            };

            // 2. Logic: Transactional Save

            using var transaction = await _context.Database.BeginTransactionAsync();

            try

            {

                // Save Parent

                _context.SupportTickets.Add(ticket);

                await _context.SaveChangesAsync();

                // Assign Generated ID to Child

                detail.TicketId = ticket.TicketId;

                // Save Child

                _context.TicketDetails.Add(detail);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

            }

            catch (Exception)

            {

                await transaction.RollbackAsync();

                throw;

            }

        }

    }

}
