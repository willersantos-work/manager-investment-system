using InvestManagerSystem.Enums;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.UserRepository
{
    public interface IUserRepository
    {
        User? FindByEmail(string email);
        IList<User> FindByType(UserTypeEnum type);
        void Save(User user);
    }
}
