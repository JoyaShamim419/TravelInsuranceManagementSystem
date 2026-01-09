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
    }
}
