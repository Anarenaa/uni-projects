using Core.Models;

namespace Services
{
    public interface IUserSession
    {
        User? CurrentUser { get; set; }
        void Logout();
    }

    public class UserSession : IUserSession
    {
        public User? CurrentUser { get; set; }

        public void Logout() => CurrentUser = null;
    }
}
