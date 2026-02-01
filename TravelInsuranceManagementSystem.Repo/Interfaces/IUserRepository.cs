using TravelInsuranceManagementSystem.Repo.Models; // Ensure this points to where your models moved
 
namespace TravelInsuranceManagementSystem.Repo.Interfaces
{
    public interface IUserRepository
    {
        User GetByEmail(string email);
        void Add(User user);
        void Update(User user);
        bool Exists(string email);
        void Save();
    }
}