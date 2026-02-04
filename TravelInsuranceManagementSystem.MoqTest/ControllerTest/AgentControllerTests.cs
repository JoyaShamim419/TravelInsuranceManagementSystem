using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;
using TravelInsuranceManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Repo.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    [TestFixture]
    public class AgentControllerTests
    {
        private Mock<IAgentService> _svc = null!;
        private Mock<SignInManager<User>> _signIn = null!;
        private AgentController _ctrl = null!;

        [SetUp]
        public void SetUp()
        {
            _svc = new Mock<IAgentService>();
            var store = new Mock<IUserStore<User>>().Object;
            var userManager = new Mock<UserManager<User>>(store, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signIn = new Mock<SignInManager<User>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
            _ctrl = new AgentController(_svc.Object, _signIn.Object);

            var user = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new System.Security.Claims.Claim[] { new System.Security.Claims.Claim("UserId", "5") }, "mock"));
            _ctrl.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        [TearDown]
        public void TearDown()
        {
            _ctrl?.Dispose();
        }

        [Test]
        public async Task Policies_Returns_View()
        {
            _svc.Setup(s => s.GetPoliciesAsync()).ReturnsAsync(new List<Policy>());
            var res = await _ctrl.Policies();
            Assert.That(res, Is.TypeOf<ViewResult>());
        }
    }
}
