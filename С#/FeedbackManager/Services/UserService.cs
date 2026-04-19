using Core.Models;
using Repositories;

namespace Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }
        public List<User> GetActiveUsers()
        {
            return _userRepository.GetAllActiveUsers();
        }
        public User? GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new Exception("User is not found");
            return user;
        }
        private string generateLogin(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;

            var translit = new Dictionary<char, string> {
                {'а',"a"}, {'б',"b"}, {'в',"v"}, {'г',"g"}, {'д',"d"}, {'е',"e"}, {'є',"ye"},
                {'ж',"zh"}, {'з',"z"}, {'и',"y"}, {'і',"i"}, {'ї',"yi"}, {'й',"y"}, {'к',"k"},
                {'л',"l"}, {'м',"m"}, {'н',"n"}, {'о',"o"}, {'п',"p"}, {'р',"r"}, {'с',"s"},
                {'т',"t"}, {'у',"u"}, {'ф',"f"}, {'х',"kh"}, {'ц',"ts"}, {'ч',"ch"}, {'ш',"sh"},
                {'щ',"shch"}, {'ь',""}, {'ю',"yu"}, {'я',"ya"}
            };

            string normalized = fullName.ToLower().Trim();
            string login = "";

            foreach (char c in normalized)
            {
                if (translit.ContainsKey(c)) login += translit[c];
                else if (c == ' ') login += "_";
                else if (char.IsLetter(c)) login += c;
            }
            return login;
        }
        private string generatePassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public int AddUser(Role role, string fullName, string number)
        {
            if(typeof(Role).IsEnumDefined(role) == false)
                throw new Exception("Посада не визначена");
            if(string.IsNullOrWhiteSpace(fullName))
                throw new Exception("Повне ім'я не може бути порожнім.");
            if(fullName.Length > 50)
                throw new Exception("Повне ім'я занадто довге.");
            if(string.IsNullOrWhiteSpace(number))
                throw new Exception("Номер телефону не може бути порожнім.");
            if(number.Length > 15)
                throw new Exception("Номер телефону занадто довгий.");

            var user = new User
            {
                Role = role,
                FullName = fullName,
                PhoneNumber = number,
                Login = generateLogin(fullName),
                Password = generatePassword(),
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            _userRepository.Add(user);
            _userRepository.SaveChanges();

            return user.Id;
        }
        public void UpdateUser(int id, string fullName, string number)
        {
            var existingUser = GetUserById(id);
            if (existingUser == null)
                throw new Exception("User is not found");

            existingUser.FullName = fullName;
            existingUser.PhoneNumber = number;
            existingUser.Login = generateLogin(fullName);
            _userRepository.SaveChanges();
        }
        public void DeactivateUser(int id)
        {
            var existingUser = _userRepository.GetById(id);
            if (existingUser == null)
                throw new Exception("User is not found");

            existingUser.IsActive = false;
            _userRepository.SaveChanges();
        }
        public List<Role> GetAllRoles()
        {
            return _userRepository.GetAllRoles().ToList();
        }
    }
}
