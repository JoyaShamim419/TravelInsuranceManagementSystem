// Additional tests for AccountController POST actions
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;
using TravelInsuranceManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Repo.Models;
using System.Threading.Tasks;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    public partial class AccountControllerTests
    {
        [Test]
        public async Task ForgotPassword_Redirects_When_UserNotFound_Shows_View()
        {
            _acc.Setup(a => a.GetUserByEmail("no@one.com")).ReturnsAsync((User?)null);
            var res = await _ctrl.ForgotPassword("no@one.com");
            Assert.That(res, Is.TypeOf<ViewResult>());
        }
    }
}
