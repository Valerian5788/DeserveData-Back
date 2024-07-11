using DAL.AppDbContextFolder;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DAL.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Email == email);
        }

        public bool CreateUser(User user)
        {
            _context.Set<User>().Add(user);
            return _context.SaveChanges() > 0;
        }

        public bool UpdatePassword(User user)
        {
            var existingUser = _context.Set<User>().FirstOrDefault(u => u.Email == user.Email);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.Password = user.Password;
            _context.Entry(existingUser).State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteUser(string email)
        {
            var user = _context.Set<User>().FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            _context.Set<User>().Remove(user);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Set<User>().ToList();
        }
    }
}
