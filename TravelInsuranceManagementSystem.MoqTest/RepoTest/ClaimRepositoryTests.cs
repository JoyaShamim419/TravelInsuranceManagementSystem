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
    public class ClaimRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private ClaimRepository _repo = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new ClaimRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task SubmitClaimAsync_Fails_If_NotOwner()
        {
            var claim = new TravelInsuranceManagementSystem.Repo.Models.Claim { PolicyId = 99, ClaimAmount = 1000m, Description = "X", IncidentType = "Other" };
            var result = await _repo.SubmitClaimAsync(claim, 1, "file.png");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task SubmitClaimAsync_Succeeds_For_Owner()
        {
            var policy = new Policy { PolicyId = 2, UserId = 3, CoverageType = "Basic", TravelStartDate = System.DateTime.Today, TravelEndDate = System.DateTime.Today.AddDays(2), PolicyStatus = PolicyStatus.ACTIVE };
            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();

            var claim = new TravelInsuranceManagementSystem.Repo.Models.Claim { PolicyId = 2, ClaimAmount = 500m, Description = "Test incident", IncidentType = "Medical" };
            var result = await _repo.SubmitClaimAsync(claim, 3, "doc.pdf");
            Assert.That(result.Success, Is.True);
            Assert.That(claim.ClaimDate.Date, Is.EqualTo(System.DateTime.Now.Date));
        }
    }
}
