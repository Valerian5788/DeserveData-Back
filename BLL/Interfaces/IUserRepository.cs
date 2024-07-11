using DAL.Entities;
using DAL.Forms.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserRepository
    {
        public User? CreateUser(CreateUserForm form);
        public bool DeleteUser(string email);
        public IEnumerable<User> GetAllUser();
        public User GetUserByEmail(string email);
        public bool UpdatePassword(UpdatePasswordForm form);
        public object? LoginUser(LoginForm form);
    }
}
