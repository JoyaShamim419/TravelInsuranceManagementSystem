using Moq;
using NUnit.Framework;
using TravelInsuranceManagementSystem.Services.Implementation;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.MoqTest.ServiceTest
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IPaymentRepository> _paymentRepoMock = null!;
        private PaymentService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _paymentRepoMock = new Mock<IPaymentRepository>();
            _service = new PaymentService(_paymentRepoMock.Object);
        }

        [Test]
        public void InitializePayment_Forwards_To_Repository()
        {
            var expected = new Payment { PaymentId = 1 };
            _paymentRepoMock.Setup(r => r.GetOrCreatePayment(5)).Returns(expected);

            var actual = _service.InitializePayment(5);

            Assert.That(actual, Is.SameAs(expected));
            _paymentRepoMock.Verify(r => r.GetOrCreatePayment(5), Times.Once);
        }

        [Test]
        public void ProcessPayment_Forwards_To_Repository()
        {
            _paymentRepoMock.Setup(r => r.ExecutePaymentProcessing(2, "4111")).Returns(true);

            var result = _service.ProcessPayment(2, "4111");

            Assert.That(result, Is.True);
            _paymentRepoMock.Verify(r => r.ExecutePaymentProcessing(2, "4111"), Times.Once);
        }

        [Test]
        public void GetPaymentDetails_Forwards_To_Repository()
        {
            var expected = new Payment { PaymentId = 9 };
            _paymentRepoMock.Setup(r => r.GetPaymentById(9)).Returns(expected);

            var actual = _service.GetPaymentDetails(9);

            Assert.That(actual, Is.SameAs(expected));
            _paymentRepoMock.Verify(r => r.GetPaymentById(9), Times.Once);
        }
    }
}
