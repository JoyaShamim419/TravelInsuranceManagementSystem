using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;
using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation

{

    public class PaymentService : IPaymentService

    {

        private readonly IPaymentRepository _paymentRepo;

        public PaymentService(IPaymentRepository paymentRepo)

        {

            _paymentRepo = paymentRepo;

        }

        public Payment InitializePayment(int policyId) =>

            _paymentRepo.GetOrCreatePayment(policyId);

        public bool ProcessPayment(int paymentId, string cardNumber) =>

            _paymentRepo.ExecutePaymentProcessing(paymentId, cardNumber);

        // New Implementation

        public Payment GetPaymentDetails(int paymentId) =>

            _paymentRepo.GetPaymentById(paymentId);

    }

}
