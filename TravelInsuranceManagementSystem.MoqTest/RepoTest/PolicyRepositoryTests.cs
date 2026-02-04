using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Repo.Data;
using TravelInsuranceManagementSystem.Repo.Implementation;
using TravelInsuranceManagementSystem.Repo.Models;
using System.Threading.Tasks;

namespace TravelInsuranceManagementSystem.MoqTest.RepoTest
{
    [TestFixture]
    public class PolicyRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private PolicyRepository _repo = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new PolicyRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateFamilyPolicyAsync_Creates_Policy()
        {
            var dto = new FamilyInsuranceDto
            {
                PolicyDetails = new PolicyDto { PlanType = "Premium", Destination = "X", TripStart = System.DateTime.Today, TripEnd = System.DateTime.Today.AddDays(5), PrimaryEmail = "a@b.com", PrimaryMobile = "1234567890" },
                Members = new System.Collections.Generic.List<MemberDto> { new MemberDto { Title = "Mr", FirstName = "A", LastName = "B", Relation = "Self", DOB = System.DateTime.Today.AddYears(-30), Mobile = "1234567890" } },
                Nominee = new NomineeDto { NomineeName = "N", NomineeRelation = "Rel", NomineeMobile = "1234567890" },
                Declarations = new DeclarationDto { IsNotPEP = true, IsResident = true }
            };

            var id = await _repo.CreateFamilyPolicyAsync(dto, 7);

            Assert.That(id, Is.GreaterThan(0));
            var saved = _context.Policies.Find(id);
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved.CoverageAmount, Is.EqualTo(50000m));
        }
    }
}
