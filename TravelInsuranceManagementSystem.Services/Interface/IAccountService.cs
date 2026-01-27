using TravelInsuranceManagementSystem.Application.Models;

namespace TravelInsuranceManagementSystem.Services.Interfaces
{
    public interface IAccountService
    {
        User Authenticate(string email, string password);
        string GenerateJwtToken(User user);
        bool RegisterUser(User user);
        bool ResetPassword(string email, string newPassword);
        User GetUserByEmail(string email);
    }
}