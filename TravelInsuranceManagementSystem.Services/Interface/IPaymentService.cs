using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Services.Interfaces
{
    public interface IPaymentService
    {
        Payment InitializePayment(int policyId);
        bool ProcessPayment(int paymentId, string cardNumber);
        // New method needed for Confirmation Page
        Payment GetPaymentDetails(int paymentId);
    }
}