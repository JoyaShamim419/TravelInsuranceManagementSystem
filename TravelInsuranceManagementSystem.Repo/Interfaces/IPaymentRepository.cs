using TravelInsuranceManagementSystem.Models;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Repo.Interfaces

{

    public interface IPaymentRepository

    {

        // Restored Method

        Payment GetPaymentById(int id);

        // Logic Methods

        Payment GetOrCreatePayment(int policyId);

        bool ExecutePaymentProcessing(int paymentId, string cardNumber);

    }

}
