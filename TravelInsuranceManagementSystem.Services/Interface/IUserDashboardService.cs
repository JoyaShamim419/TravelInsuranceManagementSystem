using Microsoft.AspNetCore.Http;
//using System.Security.Claims;
using TravelInsuranceManagementSystem.Application.Models;
namespace TravelInsuranceManagementSystem.Services.Interfaces
{
    public interface IUserDashboardService
    {
        Task<List<Claim>> GetUserClaimsAsync(int userId);
        Task<List<Policy>> GetUserPoliciesAsync(int userId);
        Task RaiseSupportTicketAsync(IFormCollection form, string userId);
    }
}
