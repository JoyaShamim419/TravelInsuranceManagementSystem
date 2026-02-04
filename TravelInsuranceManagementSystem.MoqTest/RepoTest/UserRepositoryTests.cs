using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Repo.Data;
using TravelInsuranceManagementSystem.Repo.Implementation;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.MoqTest.RepoTest
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private UserRepository _repo = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Add_And_GetByEmail()
        {
            var user = new User { Id = 1, Email = "a@b.com", FullName = "A" };
            _repo.Add(user);
            _repo.Save();

            var fetched = _repo.GetByEmail("a@b.com");
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched.Email, Is.EqualTo("a@b.com"));
        }
    }
}
