using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Repo.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<TicketDetail> TicketDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Crucial: Base call ensures Identity tables are created
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Policy>()
                .Property(p => p.PolicyStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Claim>()
                .Property(c => c.Status)
                .HasConversion<string>();
        }
    }
}