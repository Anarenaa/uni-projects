using Core.Context;
using Core.Models;

namespace Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DataContext context) : base(context) { }
        public List<User> GetAllActiveUsers()
        {
            return _dbSet.Where(u => u.IsActive).ToList();
        }
        public User? GetUserByLogin(string login)
        {
            return _dbSet.Where(u => u.IsActive).FirstOrDefault(u => u.Login == login);
        }
        public IEnumerable<Role> GetAllRoles()
        {
            return Enum.GetValues<Role>();
        }
    }
}
