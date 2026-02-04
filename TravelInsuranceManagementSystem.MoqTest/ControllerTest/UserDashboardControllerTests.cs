using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;
using TravelInsuranceManagementSystem.Services.Interfaces;
// fully qualify security claim types to avoid ambiguity with Repo Claim model
using Microsoft.AspNetCore.Http;
using TravelInsuranceManagementSystem.Repo.Models;
using System.Collections.Generic;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    [TestFixture]
    public class UserDashboardControllerTests
    {
        private Mock<IUserDashboardService> _svc = null!;
        private UserDashboardController _ctrl = null!;

        [SetUp]
        public void SetUp()
        {
            _svc = new Mock<IUserDashboardService>();
            _ctrl = new UserDashboardController(_svc.Object);

            var user = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new System.Security.Claims.Claim[] { new System.Security.Claims.Claim("UserId", "3") }, "mock"));
            _ctrl.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        [TearDown]
        public void TearDown()
        {
            _ctrl?.Dispose();
        }

        [Test]
        public async Task Dashboard_Redirects_To_SignIn_When_No_Claim()
        {
            var noUser = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity());
            _ctrl.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = noUser } };
            var res = await _ctrl.Dashboard();
            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public async Task Dashboard_Returns_View_When_User()
        {
            _svc.Setup(s => s.GetDashboardSummaryAsync(3)).ReturnsAsync(new UserDashboardViewModel());
            var res = await _ctrl.Dashboard();
            Assert.That(res, Is.TypeOf<ViewResult>());
        }
    }
}
