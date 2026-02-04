using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Services.Implementation;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TravelInsuranceManagementSystem.MoqTest.ServiceTest
{
    [TestFixture]
    public class AgentServiceTests
    {
        private Mock<IAgentRepository> _repoMock = null!;
        private AgentService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IAgentRepository>();
            _service = new AgentService(_repoMock.Object);
        }

        [Test]
        public async Task GetPoliciesAsync_Forwards()
        {
            _repoMock.Setup(r => r.GetPoliciesWithMembersAsync()).ReturnsAsync(new List<Policy>());
            var res = await _service.GetPoliciesAsync();
            Assert.That(res, Is.Not.Null);
        }
    }
}
