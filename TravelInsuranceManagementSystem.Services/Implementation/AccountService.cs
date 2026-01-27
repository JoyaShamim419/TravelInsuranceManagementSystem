using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims; // For ClaimTypes

using System.Text;

using TravelInsuranceManagementSystem.Application.Models;

using TravelInsuranceManagementSystem.Repo.Interfaces;

using TravelInsuranceManagementSystem.Services.Interfaces;

namespace TravelInsuranceManagementSystem.Services.Implementation

{

    public class AccountService : IAccountService

    {

        private readonly IUserRepository _userRepo;

        private readonly IPasswordHasher<User> _passwordHasher;

        private readonly IConfiguration _config;

        public AccountService(IUserRepository userRepo, IPasswordHasher<User> passwordHasher, IConfiguration config)

        {

            _userRepo = userRepo;

            _passwordHasher = passwordHasher;

            _config = config;

        }

        public User Authenticate(string email, string password)

        {

            var user = _userRepo.GetByEmail(email);

            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return result == PasswordVerificationResult.Success ? user : null;

        }

        public string GenerateJwtToken(User user)

        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            // FIXED: Using Full Namespace path to avoid ambiguity with the 'Claim' Model

            var tokenDescriptor = new SecurityTokenDescriptor

            {

                Subject = new ClaimsIdentity(new[]

                {

                    new System.Security.Claims.Claim(ClaimTypes.Name, user.FullName),

                    new System.Security.Claims.Claim(ClaimTypes.Email, user.Email),

                    new System.Security.Claims.Claim(ClaimTypes.Role, user.Role),

                    new System.Security.Claims.Claim("UserId", user.Id.ToString())

                }),

                Expires = DateTime.UtcNow.AddHours(2),

                Issuer = _config["Jwt:Issuer"],

                Audience = _config["Jwt:Audience"],

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public bool RegisterUser(User user)

        {

            if (_userRepo.Exists(user.Email)) return false;

            if (user.Email.ToLower().Contains("@admin")) user.Role = "Admin";

            else if (user.Email.ToLower().Contains("@agent")) user.Role = "Agent";

            else user.Role = "User";

            user.Password = _passwordHasher.HashPassword(user, user.Password);

            _userRepo.Add(user);

            _userRepo.Save();

            return true;

        }

        public bool ResetPassword(string email, string newPassword)

        {

            var user = _userRepo.GetByEmail(email);

            if (user == null) return false;

            user.Password = _passwordHasher.HashPassword(user, newPassword);

            _userRepo.Update(user);

            _userRepo.Save();

            return true;

        }

        public User GetUserByEmail(string email) => _userRepo.GetByEmail(email);

    }

}
