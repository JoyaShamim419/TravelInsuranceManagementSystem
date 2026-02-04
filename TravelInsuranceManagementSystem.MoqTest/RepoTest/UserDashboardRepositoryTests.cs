using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Repo.Data;
using TravelInsuranceManagementSystem.Repo.Implementation;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelInsuranceManagementSystem.MoqTest.RepoTest
{
    [TestFixture]
    public class UserDashboardRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private UserDashboardRepository _repo = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new UserDashboardRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetPoliciesByUserIdAsync_Returns_List()
        {
            _context.Policies.Add(new Policy { PolicyId = 11, UserId = 2, CoverageType = "Basic", TravelStartDate = System.DateTime.Today, TravelEndDate = System.DateTime.Today.AddDays(1), PolicyStatus = PolicyStatus.ACTIVE });
            await _context.SaveChangesAsync();

            var list = await _repo.GetPoliciesByUserIdAsync(2);
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task RaiseSupportTicketAsync_Creates_Ticket_And_Detail()
        {
            var form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Description", "Help" },
                { "Subject", "S" },
                { "Category", "C" },
                { "Priority", "High" },
                { "RelatedId", "0" },
                { "ContactMethod", "Email" },
                { "ContactDetail", "a@b.com" }
            });

            // InMemory provider ignores transactions and may log a warning. Configure context to allow that.
            await _repo.RaiseSupportTicketAsync(form, 3);

            var tickets = await _context.SupportTickets.ToListAsync();
            var details = await _context.TicketDetails.ToListAsync();

            Assert.That(tickets.Count, Is.EqualTo(1));
            Assert.That(details.Count, Is.EqualTo(1));
        }
    }
}
