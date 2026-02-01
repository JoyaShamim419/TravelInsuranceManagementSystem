using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Repo.Models;

namespace TravelInsuranceManagementSystem.Services.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> Authenticate(string email, string password);
        Task<IdentityResult> RegisterUser(User user, string password);
        Task<User> GetUserByEmail(string email);
        Task<string> GeneratePasswordResetToken(User user);
        Task<IdentityResult> ResetPassword(User user, string token, string newPassword);
    }
}