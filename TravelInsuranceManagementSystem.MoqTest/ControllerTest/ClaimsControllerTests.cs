using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;
using TravelInsuranceManagementSystem.Services.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
// Avoid ambiguous Claim type by fully-qualifying security claims when needed
using System.Threading.Tasks;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    [TestFixture]
    public class ClaimsControllerTests
    {
        private Mock<IClaimService> _svc = null!;
        private Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment> _env = null!;
        private ClaimsController _ctrl = null!;

        [SetUp]
        public void SetUp()
        {
            _svc = new Mock<IClaimService>();
            _env = new Mock<IWebHostEnvironment>();
            _ctrl = new ClaimsController(_svc.Object, _env.Object);

            var user = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new[] { new System.Security.Claims.Claim("UserId","4") }, "mock"));
            _ctrl.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            // Ensure TempData is available to avoid NullReference when controller sets TempData
            _ctrl.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                _ctrl.ControllerContext.HttpContext,
                new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>().Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _ctrl?.Dispose();
        }

        [Test]
        public void Create_Get_Returns_View()
        {
            var res = _ctrl.Create();
            Assert.That(res, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Create_Post_Redirects_When_Success()
        {
            var claim = new TravelInsuranceManagementSystem.Repo.Models.Claim { PolicyId = 2, IncidentType = "X", Description = "D", ClaimAmount = 10 };
            _svc.Setup(s => s.SubmitClaimAsync(It.IsAny<TravelInsuranceManagementSystem.Repo.Models.Claim>(), 4, (string?)null)).ReturnsAsync((true, "Success"));

            var res = await _ctrl.Create(claim);
            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
        }
    }
}
