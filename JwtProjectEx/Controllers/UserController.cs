using JwtProjectEx.ApiResponses;
using JwtProjectEx.Dtos;
using JwtProjectEx.Entities;
using JwtProjectEx.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtProjectEx.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<ActionResult<ApplicationUser>> Register(RegisterUserDto registerUserDto)
        {
            var result = _userRepo.RegisterUserAsync(registerUserDto);
            return Ok(result);

        }

        public async Task<ActionResult<LogInUserResponse>> LogIn(LogInUserDto logInUserDto)
        {
           var result = _userRepo.LogInUserAsync(logInUserDto);
            return Ok(result);
        }
    }
}
