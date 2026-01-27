using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IPolicyRepository _policyRepo;

        public PaymentService(IPaymentRepository paymentRepo, IPolicyRepository policyRepo)
        {
            _paymentRepo = paymentRepo;
            _policyRepo = policyRepo;
        }

        public Payment InitializePayment(int policyId)
        {
            var existing = _paymentRepo.GetPendingPaymentByPolicyId(policyId);
            if (existing != null) return existing;

            // Fixes CS0103: 'policy' now exists in this context
            var policy = _policyRepo.GetById(policyId);
            if (policy == null) return null;

            var newPayment = new Payment
            {
                PolicyId = policyId,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.PENDING,
                PaymentAmount = (policy.CoverageType == "Premium") ? 5000 : 2500
            };

            _paymentRepo.AddPayment(newPayment);
            _paymentRepo.Save();
            return newPayment;
        }

        public bool ProcessPayment(int paymentId, string cardNumber)
        {
            var payment = _paymentRepo.GetPaymentById(paymentId);
            if (payment == null) return false;

            // Fixes CS0103: 'isFailed' is explicitly declared
            bool isFailed = payment.PaymentAmount <= 0 ||
                           (!string.IsNullOrEmpty(cardNumber) && cardNumber.Replace(" ", "") == "0000000000000000");

            if (isFailed)
            {
                payment.PaymentStatus = PaymentStatus.FAILED;
            }
            else
            {
                payment.PaymentStatus = PaymentStatus.SUCCESS;
            }

            payment.PaymentDate = DateTime.Now;
            _paymentRepo.UpdatePayment(payment);
            _paymentRepo.Save();

            return payment.PaymentStatus == PaymentStatus.SUCCESS;
        }
    }
}