using TravelInsuranceManagementSystem.Models;
namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IPaymentRepository
    {
        Payment GetPaymentById(int id);
        Payment GetPendingPaymentByPolicyId(int policyId);
        void AddPayment(Payment payment);
        void UpdatePayment(Payment payment);
        void Save();
    }
}
