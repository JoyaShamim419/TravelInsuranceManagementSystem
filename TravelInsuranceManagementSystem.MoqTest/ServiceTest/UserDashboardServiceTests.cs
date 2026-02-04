using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Services.Implementation;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using Microsoft.AspNetCore.Http;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.MoqTest.ServiceTest
{
    [TestFixture]
    public class UserDashboardServiceTests
    {
        private Mock<IUserDashboardRepository> _repoMock = null!;
        private UserDashboardService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IUserDashboardRepository>();
            _service = new UserDashboardService(_repoMock.Object);
        }

        [Test]
        public async Task GetUserClaimsAsync_Forwards()
        {
            _repoMock.Setup(r => r.GetClaimsByUserIdAsync(1)).ReturnsAsync(new List<Claim>());
            var list = await _service.GetUserClaimsAsync(1);
            Assert.That(list, Is.Not.Null);
        }

        [Test]
        public async Task RaiseSupportTicketAsync_Forwards()
        {
            var form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());
            _repoMock.Setup(r => r.RaiseSupportTicketAsync(form, 2)).Returns(Task.CompletedTask);
            await _service.RaiseSupportTicketAsync(form, 2);
            _repoMock.Verify(r => r.RaiseSupportTicketAsync(form, 2), Times.Once);
        }
    }
}
