using Microsoft.EntityFrameworkCore;
using System;
using TravelInsuranceManagementSystem.Application.Models;

namespace TravelInsuranceManagementSystem.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This ensures the Enum is stored as a String in the DB instead of an Integer
            modelBuilder.Entity<Policy>()
                .Property(p => p.PolicyStatus)
                .HasConversion<string>();
            modelBuilder.Entity<Claim>()
                .Property(c => c.Status)
                .HasConversion<string>();
        }
    }
}
