using JwtProjectEx.ApiResponses;
using JwtProjectEx.Dtos;
using JwtProjectEx.Entities;
using JwtProjectEx.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtProjectEx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ApplicationUser>> Register(RegisterUserDto registerUserDto)
        {
            var result =await _userRepo.RegisterUserAsync(registerUserDto);
            return Ok(result);

        }
        [HttpPost("Login")]
        public async Task<ActionResult<LogInUserResponse>> LogIn(LogInUserDto logInUserDto)
        {
           var result = await _userRepo.LogInUserAsync(logInUserDto);
            return Ok(result);
        }
    }
}
