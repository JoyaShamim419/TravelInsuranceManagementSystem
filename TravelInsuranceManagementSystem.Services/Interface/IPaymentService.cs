using TravelInsuranceManagementSystem.Models;

namespace TravelInsuranceManagementSystem.Services.Interfaces
{
    public interface IPaymentService
    {
        Payment InitializePayment(int policyId);
        bool ProcessPayment(int paymentId, string cardNumber);
        Payment GetPaymentDetails(int paymentId);
    }
}