using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Services.Implementation;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.MoqTest.ServiceTest
{
    [TestFixture]
    public class PolicyServiceTests
    {
        private Mock<IPolicyRepository> _policyRepoMock = null!;
        private PolicyService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _policyRepoMock = new Mock<IPolicyRepository>();
            _service = new PolicyService(_policyRepoMock.Object);
        }

        [Test]
        public async Task CreateFamilyPolicyAsync_Forwards_To_Repository()
        {
            var dto = new FamilyInsuranceDto();
            _policyRepoMock.Setup(r => r.CreateFamilyPolicyAsync(dto, 7)).ReturnsAsync(42);

            var id = await _service.CreateFamilyPolicyAsync(dto, 7);

            Assert.That(id, Is.EqualTo(42));
            _policyRepoMock.Verify(r => r.CreateFamilyPolicyAsync(dto, 7), Times.Once);
        }
    }
}
