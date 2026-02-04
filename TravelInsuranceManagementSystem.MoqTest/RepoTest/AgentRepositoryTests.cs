using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Repo.Data;
using TravelInsuranceManagementSystem.Repo.Implementation;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models;
using System.Threading.Tasks;

namespace TravelInsuranceManagementSystem.MoqTest.RepoTest
{
    [TestFixture]
    public class AgentRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private AgentRepository _repo = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new AgentRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetPaymentsWithPolicyAsync_Returns_List()
        {
            var p = new Policy { PolicyId = 60, CoverageType = "Basic", TravelStartDate = System.DateTime.Today, TravelEndDate = System.DateTime.Today.AddDays(1), PolicyStatus = PolicyStatus.ACTIVE };
            _context.Policies.Add(p);
            await _context.SaveChangesAsync();

            _context.Payments.Add(new TravelInsuranceManagementSystem.Models.Payment { PaymentId = 21, PaymentAmount = 100m, PaymentStatus = TravelInsuranceManagementSystem.Models.PaymentStatus.SUCCESS, PaymentDate = System.DateTime.Now, PolicyId = p.PolicyId, Policy = p });
            await _context.SaveChangesAsync();

            var dbCount = await _context.Payments.CountAsync();
            Assert.That(dbCount, Is.GreaterThan(0), "Payment was not saved to the test context");

            var list = await _repo.GetPaymentsWithPolicyAsync();
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(dbCount));
        }
    }
}
