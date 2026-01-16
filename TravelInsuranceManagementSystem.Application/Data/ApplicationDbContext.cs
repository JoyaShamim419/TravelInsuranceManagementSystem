using Microsoft.EntityFrameworkCore;
using System;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This ensures the Enum is stored as a String in the DB instead of an Integer
            modelBuilder.Entity<Policy>()
                .Property(p => p.PolicyStatus)
                .HasConversion<string>();
        }
    }
}
