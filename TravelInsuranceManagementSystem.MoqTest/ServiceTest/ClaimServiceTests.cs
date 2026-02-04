using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Services.Implementation;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.MoqTest.ServiceTest
{
    [TestFixture]
    public class ClaimServiceTests
    {
        private Mock<IClaimRepository> _claimRepoMock = null!;
        private ClaimService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _claimRepoMock = new Mock<IClaimRepository>();
            _service = new ClaimService(_claimRepoMock.Object);
        }

        [Test]
        public async Task SubmitClaimAsync_Forwards_To_Repository()
        {
            var claim = new Claim();
            _claimRepoMock.Setup(r => r.SubmitClaimAsync(claim, 3, "file.png")).ReturnsAsync((true, "ok"));

            var result = await _service.SubmitClaimAsync(claim, 3, "file.png");

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("ok"));
            _claimRepoMock.Verify(r => r.SubmitClaimAsync(claim, 3, "file.png"), Times.Once);
        }
    }
}
