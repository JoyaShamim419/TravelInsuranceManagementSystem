using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Repo.Data;
using TravelInsuranceManagementSystem.Repo.Implementation;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Models;
using System.Linq;

namespace TravelInsuranceManagementSystem.MoqTest.RepoTest
{
    [TestFixture]
    public class PaymentRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private PaymentRepository _repo = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new PaymentRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetOrCreatePayment_Creates_When_NoPendingExists()
        {
            var policy = new Policy { PolicyId = 1, CoverageType = "Basic", Members = new System.Collections.Generic.List<PolicyMember>() };
            _context.Policies.Add(policy);
            _context.SaveChanges();
            // Ensure navigation properties are tracked
            var storedPolicy = _context.Policies.Find(policy.PolicyId);

            var payment = _repo.GetOrCreatePayment(1);

            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.PolicyId, Is.EqualTo(1));
            Assert.That(payment.PaymentStatus, Is.EqualTo(TravelInsuranceManagementSystem.Models.PaymentStatus.PENDING));
        }

        [Test]
        public void ExecutePaymentProcessing_Fails_On_InvalidCard()
        {
            var payment = new TravelInsuranceManagementSystem.Models.Payment { PaymentId = 10, PolicyId = 5, PaymentAmount = 100m, PaymentStatus = TravelInsuranceManagementSystem.Models.PaymentStatus.PENDING };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            var result = _repo.ExecutePaymentProcessing(10, "0000000000000000");

            Assert.That(result, Is.False);
            var updated = _context.Payments.Find(10);
            Assert.That(updated.PaymentStatus, Is.EqualTo(TravelInsuranceManagementSystem.Models.PaymentStatus.FAILED));
        }
    }
}
