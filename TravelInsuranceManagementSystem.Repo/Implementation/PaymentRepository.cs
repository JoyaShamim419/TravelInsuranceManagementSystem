using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Interfaces;
namespace TravelInsuranceManagementSystem.Repo.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context) { _context = context; }
        public Payment GetPaymentById(int id) => _context.Payments.Find(id);

        public Payment GetPendingPaymentByPolicyId(int policyId) =>
            _context.Payments.FirstOrDefault(p => p.PolicyId == policyId && p.PaymentStatus == PaymentStatus.PENDING);

        public void AddPayment(Payment payment) => _context.Payments.Add(payment);

        public void UpdatePayment(Payment payment) => _context.Payments.Update(payment);

        public void Save() => _context.SaveChanges();
    }
}
