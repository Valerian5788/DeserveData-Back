using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserService
    {
        public User? GetUserByEmail(string email);
        public bool CreateUser(User user);
        public bool UpdatePassword(User user);
        public bool DeleteUser(string email);
        public IEnumerable<User> GetAllUsers();
    }
}
