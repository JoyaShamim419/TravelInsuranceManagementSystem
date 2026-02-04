using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    [TestFixture]
    public class HomeControllerTests
    {
        [TearDown]
        public void TearDown()
        {
            // No resources to dispose for HomeController tests created inline
        }
        [Test]
        public void Index_Returns_View()
        {
            var ctrl = new HomeController { ControllerContext = new ControllerContext { HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() } };
            var res = ctrl.Index();
            Assert.That(res, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void Error_Returns_View_With_Model()
        {
            var ctrl = new HomeController { ControllerContext = new ControllerContext { HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() } };
            var res = ctrl.Error();
            Assert.That(res, Is.TypeOf<ViewResult>());
            var view = (ViewResult)res;
            Assert.That(view.Model, Is.Not.Null, "Model should not be null");
        }
    }
}
