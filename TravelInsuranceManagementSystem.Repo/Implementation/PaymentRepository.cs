using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Data;
using TravelInsuranceManagementSystem.Repo.Interfaces;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Repo.Implementation

{

    public class PaymentRepository : IPaymentRepository

    {

        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        // --- RESTORED METHOD ---

        public Payment GetPaymentById(int id)

        {

            return _context.Payments.Find(id);

        }

        // --- EXISTING LOGIC METHODS ---

        public Payment GetOrCreatePayment(int policyId)

        {

            var existing = _context.Payments

                .FirstOrDefault(p => p.PolicyId == policyId && p.PaymentStatus == PaymentStatus.PENDING);

            if (existing != null) return existing;

            var policy = _context.Policies.Find(policyId);

            if (policy == null) return null;

            var newPayment = new Payment

            {

                PolicyId = policyId,

                PaymentDate = DateTime.Now,

                PaymentStatus = PaymentStatus.PENDING,

                PaymentAmount = (policy.CoverageType == "Premium") ? 5000 : 2500

            };

            _context.Payments.Add(newPayment);

            _context.SaveChanges();

            return newPayment;

        }

        public bool ExecutePaymentProcessing(int paymentId, string cardNumber)

        {

            var payment = _context.Payments.Find(paymentId);

            if (payment == null) return false;

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

            _context.Payments.Update(payment);

            _context.SaveChanges();

            return payment.PaymentStatus == PaymentStatus.SUCCESS;

        }

    }

}
