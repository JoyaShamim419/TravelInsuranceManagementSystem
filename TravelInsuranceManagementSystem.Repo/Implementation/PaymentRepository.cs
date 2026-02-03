using Microsoft.EntityFrameworkCore; // Required for .Include()

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

        public Payment GetPaymentById(int id)

        {

            return _context.Payments

                .Include(p => p.Policy) // Include Policy for Confirmation details

                .FirstOrDefault(p => p.PaymentId == id);

        }

        public Payment GetOrCreatePayment(int policyId)

        {

            // 1. Check if a Pending payment already exists

            var existing = _context.Payments

                .Include(p => p.Policy)

                .FirstOrDefault(p => p.PolicyId == policyId && p.PaymentStatus == PaymentStatus.PENDING);

            if (existing != null) return existing;

            // 2. Fetch Policy AND Members to calculate cost

            var policy = _context.Policies

                .Include(p => p.Members) // Load members list

                .FirstOrDefault(p => p.PolicyId == policyId);

            if (policy == null) return null;

            // 3. Dynamic Calculation Logic

            // Count members (if list is empty/null, assume 1 person)

            int memberCount = (policy.Members != null && policy.Members.Any())

                              ? policy.Members.Count

                              : 1;

            // Define Rates

            decimal ratePerPerson = 0;

            if (policy.CoverageType == "Premium")

            {

                ratePerPerson = 6000; // Premium Rate

            }

            else

            {

                ratePerPerson = 4000; // Basic Rate (As requested)

            }

            decimal totalAmount = ratePerPerson * memberCount;

            // 4. Create Payment Record

            var newPayment = new Payment

            {

                PolicyId = policyId,

                PaymentDate = DateTime.Now,

                PaymentStatus = PaymentStatus.PENDING,

                PaymentAmount = totalAmount // Calculated Value

            };

            _context.Payments.Add(newPayment);

            _context.SaveChanges();

            // Load reference for view

            newPayment.Policy = policy;

            return newPayment;

        }

        public bool ExecutePaymentProcessing(int paymentId, string cardNumber)

        {

            var payment = _context.Payments.Find(paymentId);

            if (payment == null) return false;

            // Simple validation simulation

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
