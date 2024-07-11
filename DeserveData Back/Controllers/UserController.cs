using BLL.Interfaces;
using DAL.Entities;
using DAL.Forms.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepositoryBLL;
        public UserController(IUserRepository userRepositoryBLL)
        {
            _userRepositoryBLL = userRepositoryBLL;
        }
        [HttpPost("CreateUser")]
        [Authorize("Admin")]
        public IActionResult CreateUser(CreateUserForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_userRepositoryBLL.CreateUser(form));
        }
        [HttpGet("GetUserByEmail/{email}")]
        [Authorize("Admin")]
        public IActionResult GetUserByEmail(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = _userRepositoryBLL.GetUserByEmail(email);
            return Ok(user);
        }
        [HttpGet("GetAllUser")]
        [Authorize("Admin")]
        public IActionResult GetAllUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_userRepositoryBLL.GetAllUser());
        }

        [HttpPatch("UpdatePassword")]
        public IActionResult UpdatePassword(UpdatePasswordForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_userRepositoryBLL.UpdatePassword(form));
        }
        [HttpDelete("DeleteUser/{email}")]
        [Authorize("Admin")]
        public IActionResult DeleteUser(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_userRepositoryBLL.DeleteUser(email));
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                object? token = _userRepositoryBLL.LoginUser(form);
                if (token != null)
                {
                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Informations de connexion obsolète");
                }

            }
        }
        [HttpGet]
        [Route("test")]
        public IActionResult Get()
        {
            return Ok(new { message = "Test réussi !" });
        }
    }
}
