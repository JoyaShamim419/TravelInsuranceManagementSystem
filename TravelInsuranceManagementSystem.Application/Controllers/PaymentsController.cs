using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. CHECKOUT PAGE
        // ==========================================
        public IActionResult Checkout(int insuranceId)
        {
            var policy = _context.Policies.FirstOrDefault(p => p.PolicyId == insuranceId);

            if (policy == null)
            {
                return NotFound("Error: Policy not found.");
            }

            // Prevent duplicate pending payments
            var existingPayment = _context.Payments
                .FirstOrDefault(p => p.PolicyId == insuranceId && p.PaymentStatus == PaymentStatus.PENDING);

            if (existingPayment != null)
            {
                return View(existingPayment);
            }

            var newPayment = new Payment
            {
                PolicyId = insuranceId,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.PENDING,
                // Simple logic: Premium is 5000, others 2500
                PaymentAmount = (policy.CoverageType == "Premium") ? 5000 : 2500
            };

            _context.Payments.Add(newPayment);
            _context.SaveChanges();

            return View(newPayment);
        }

        // ==========================================
        // 2. PROCESS PAYMENT (With Failure Logic)
        // ==========================================
        [HttpPost]
        public IActionResult ProcessPayment(int paymentId, string paymentMethod, string CardNumber)
        {
            var payment = _context.Payments.Find(paymentId);

            if (payment == null)
            {
                return NotFound("Error: Payment record not found.");
            }

            // --- FAILURE TRIGGER 1: Security Check ---
            // If amount is 0 or negative (hacker attempt), fail immediately.
            if (payment.PaymentAmount <= 0)
            {
                payment.PaymentStatus = PaymentStatus.FAILED;
                payment.PaymentDate = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("PaymentFailed");
            }

            // --- FAILURE TRIGGER 2: Simulation Check ---
            // If Card Number is "0000 0000 0000 0000", force a decline.
            if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Replace(" ", "") == "0000000000000000")
            {
                payment.PaymentStatus = PaymentStatus.FAILED;
                payment.PaymentDate = DateTime.Now;
                _context.SaveChanges();

                // Redirect to the Failed Page
                return RedirectToAction("PaymentFailed");
            }

            // --- SUCCESS LOGIC ---
            // If checks pass, mark as Success
            payment.PaymentStatus = PaymentStatus.SUCCESS;
            payment.PaymentDate = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("PaymentConfirmation", new { id = paymentId });
        }

        // ==========================================
        // 3. SUCCESS PAGE
        // ==========================================
        public IActionResult PaymentConfirmation(int id)
        {
            return View();
        }

        // ==========================================
        // 4. FAILED PAGE
        // ==========================================
        public IActionResult PaymentFailed()
        {
            return View();
        }
    }
}