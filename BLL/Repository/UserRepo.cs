using BLL.HashPassword;
using BLL.Interfaces;
using BLL.TokenManager;
using DAL.Entities;
using DAL.Forms.Users;
using DAL.Interfaces;
using BLL.Mappers;

namespace BLL.Repository
{
    public class UserRepo : IUserRepository
    {
        private readonly IUserService _userService;
        public UserRepo(IUserService userService)
        {
            _userService = userService;
        }

        public User? CreateUser(CreateUserForm form)
        {
            User? u = GetUserByEmail(form.Email);

            if (u is null)
            {
                User newUser = ToUser.CreateToUser(form);
                newUser.Password = HashPasswordBcrypt.HashPassword(newUser.Password);
                if (_userService.CreateUser(newUser))
                {
                    return newUser;
                }
                return null;
            }
            else
            {
                Console.WriteLine("Déja existant");
                return null;
            }
        }

        public bool DeleteUser(string email)
        {
            User? u = GetUserByEmail(email);
            if (u is not null)
            {
                return _userService.DeleteUser(email);
            }
            return false;
        }

        public IEnumerable<User> GetAllUser()
        {
            return _userService.GetAllUsers();
        }

        public User? GetUserByEmail(string email)
        {
            return _userService.GetUserByEmail(email);
        }

        public bool UpdatePassword(UpdatePasswordForm form)
        {
            User u = GetUserByEmail(form.Email);

            if (u is not null)
            {
                if (HashPasswordBcrypt.VerifyPasword(form.Password, u.Password))
                {
                    u.Password = HashPasswordBcrypt.HashPassword(form.NewPassword);
                    return _userService.UpdatePassword(u);
                }
                return false;
            }
            return false;
        }


        public object? LoginUser(LoginForm form)
        {
            User? s = GetUserByEmail(form.Email);

            if (s != null)
            {
                if (HashPasswordBcrypt.VerifyPasword(form.Password, s.Password))
                {
                    return new { token = GenerateTokenManager.GenerateToken(s) };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
