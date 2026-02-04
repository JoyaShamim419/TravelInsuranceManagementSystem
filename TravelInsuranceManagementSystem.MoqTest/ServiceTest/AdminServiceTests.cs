using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Services.Implementation;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Repo.Data;
using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Repo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelInsuranceManagementSystem.MoqTest.ServiceTest
{
    [TestFixture]
    public class AdminServiceTests
    {
        private Mock<IAdminRepository> _repoMock = null!;
        private Mock<ApplicationDbContext> _contextMock = null!;
        private Mock<UserManager<User>> _userManagerMock = null!;
        private AdminService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IAdminRepository>();
            // UserManager is complex; tests will only verify repository calls where possible
            _service = new AdminService(_repoMock.Object, null!, null!);
        }

        [Test]
        public async Task GetPaymentHistoryAsync_Forwards()
        {
            // Use the Repo's model namespace for Payment
            _repoMock.Setup(r => r.GetAllPaymentsWithPoliciesAsync()).ReturnsAsync(new List<TravelInsuranceManagementSystem.Models.Payment>());
            var res = await _service.GetPaymentHistoryAsync();
            Assert.That(res, Is.Not.Null);
        }
    }
}
