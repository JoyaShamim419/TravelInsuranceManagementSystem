using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;
using TravelInsuranceManagementSystem.Services.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;
using System.Threading.Tasks;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    [TestFixture]
    public class InsuranceControllerTests
    {
        private Mock<IPolicyService> _svc = null!;
        private InsuranceController _ctrl = null!;

        [SetUp]
        public void SetUp()
        {
            _svc = new Mock<IPolicyService>();
            _ctrl = new InsuranceController(_svc.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _ctrl?.Dispose();
        }

        [Test]
        public void FamilyInsurance_Get_Returns_View()
        {
            var res = _ctrl.FamilyInsurance();
            Assert.That(res, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task CreateFamily_Returns_BadRequest_When_Null()
        {
            var res = await _ctrl.CreateFamily(null);
            Assert.That(res, Is.TypeOf<BadRequestObjectResult>());
        }
    }
}
