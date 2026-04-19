using Core.Models;
using Repositories;

namespace Services
{
    public class AuthenticationService
    {
        private UserRepository _userRepository;
        public AuthenticationService(UserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public User? Authenticate(string login, string password)
        {
            var user = _userRepository.GetUserByLogin(login);
            if (user == null)
                throw new Exception("Login is invalid");

            if (user != null && user.Password != password)
                throw new Exception("Password is invalid");

            return user;
        }
    }

}
