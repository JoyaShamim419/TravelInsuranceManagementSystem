using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Application.Controllers;
using TravelInsuranceManagementSystem.Services.Interfaces;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.MoqTest.ControllerTest
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private Mock<IPaymentService> _svc = null!;
        private PaymentsController _ctrl = null!;

        [SetUp]
        public void SetUp()
        {
            _svc = new Mock<IPaymentService>();
            _ctrl = new PaymentsController(_svc.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _ctrl?.Dispose();
        }

        [Test]
        public void Checkout_NotFound_When_PolicyMissing()
        {
            _svc.Setup(s => s.InitializePayment(5)).Returns((Payment?)null);
            var res = _ctrl.Checkout(5);
            Assert.That(res, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public void PaymentConfirmation_NotFound_When_Missing()
        {
            _svc.Setup(s => s.GetPaymentDetails(2)).Returns((Payment?)null);
            var res = _ctrl.PaymentConfirmation(2);
            Assert.That(res, Is.TypeOf<NotFoundResult>());
        }
    }
}
