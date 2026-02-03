using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IPaymentRepository
    {
        Payment GetPaymentById(int id);
        Payment GetOrCreatePayment(int policyId);
        bool ExecutePaymentProcessing(int paymentId, string cardNumber);
    }
}