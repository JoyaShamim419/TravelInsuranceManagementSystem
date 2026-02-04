using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;
using TravelInsuranceManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Repo.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    [TestFixture]
    public partial class AccountControllerTests
    {
        private Mock<IAccountService> _acc = null!;
        private Mock<SignInManager<User>> _signIn = null!;
        private Mock<UserManager<User>> _userManager = null!;
        private AccountController _ctrl = null!;
        [TearDown]
        public void TearDownController()
        {
            _ctrl?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _acc = new Mock<IAccountService>();
            var store = new Mock<IUserStore<User>>().Object;
            _userManager = new Mock<UserManager<User>>(store, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signIn = new Mock<SignInManager<User>>(_userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            _ctrl = new AccountController(_acc.Object, _signIn.Object, _userManager.Object);

            // Ensure Controller.User and Identity are non-null for the GET SignIn() path
            _ctrl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity())
                }
            };
        }

        [Test]
        public void SignIn_Get_Returns_View()
        {
            var res = _ctrl.SignIn();
            Assert.That(res, Is.TypeOf<ViewResult>());
        }
    }
}
