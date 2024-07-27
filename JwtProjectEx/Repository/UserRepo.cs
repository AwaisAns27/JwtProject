using JwtProjectEx.ApiResponses;
using JwtProjectEx.Data;
using JwtProjectEx.Dtos;
using JwtProjectEx.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtProjectEx.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<LogInUserResponse> LogInUserAsync(LogInUserDto logInUserDto)
        {
           var userInDb = await _context.ApplicationUsers.SingleOrDefaultAsync(au =>au.Email== logInUserDto.Email);

            if (userInDb == null)
            {
                return new LogInUserResponse() { Flag = false, Message = "Sorry, User not found!!!" };
            }
            bool isPasswordVerified = BCrypt.Net.BCrypt.Verify(logInUserDto.Password, userInDb.Password);
            if (isPasswordVerified)
            {
                return new LogInUserResponse() { Flag = true, Message = "You have been logged in successfully", Token="" };
            }
            else
            {
                return new LogInUserResponse() { Flag = false, Message = "Invalid Credential" };
            }


        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
          var userInDb = await _context.ApplicationUsers.SingleOrDefaultAsync(au => au.Email == registerUserDto.Email);
            if (userInDb == null)
            {
                 return new RegisterUserResponse() {Flag = false, Message="User Already Exists" };
            }

            ApplicationUser applicationUser = new() 
            {
                Email = registerUserDto.Email,
                Name = registerUserDto.Name,
                Password= BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password)
            };
            await _context.ApplicationUsers.AddAsync(applicationUser);
            await _context.SaveChangesAsync();

            return new RegisterUserResponse() { Flag = true, Message = "You have been register Successfully" };
        }
    }
}
