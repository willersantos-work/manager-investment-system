using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Database;
using InvestManagerSystem.Models;

namespace InvestManagerSystem.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public User? FindByEmail(string email)
        {
            return _context.User.SingleOrDefault(u => u.Email == email);
        }

        public IList<User> FindByType(UserTypeEnum type)
        {
            return _context.User.Where(u => u.Type == type).ToList();
        }

        public void Save(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }
    }
}
