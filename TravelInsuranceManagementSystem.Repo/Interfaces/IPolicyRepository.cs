using TravelInsuranceManagementSystem.Application.Models;
namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IPolicyRepository
    {

        Policy GetById(int id);
        Task AddPolicyAsync(Policy policy);
        Task SaveAsync();
    }
}
