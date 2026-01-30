using Microsoft.AspNetCore.Identity;
using TravelInsuranceManagementSystem.Application.Models;
using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> Authenticate(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return SignInResult.Failed;
            return await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> RegisterUser(User user, string password)
        {
            if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";
            else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";
            else user.Role = "User";

            user.UserName = user.Email;
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<string> GeneratePasswordResetToken(User user) => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityResult> ResetPassword(User user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}