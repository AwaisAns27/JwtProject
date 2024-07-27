using JwtProjectEx.ApiResponses;
using JwtProjectEx.Data;
using JwtProjectEx.Dtos;
using JwtProjectEx.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtProjectEx.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepo(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                return new LogInUserResponse() { Flag = true, Message = "You have been logged in successfully", Token=GenerateJwtToken(userInDb) };
            }
            else
            {
                return new LogInUserResponse() { Flag = false, Message = "Invalid Credential" };
            }
        }
        private string GenerateJwtToken(ApplicationUser userFromDb)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

            Claim[] claims =

            {
                new Claim(ClaimTypes.Name,userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromDb.Name),
                new Claim(ClaimTypes.Email  ,userFromDb.Email)
            };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims : claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(1)
                
                );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var jwtToken =handler.WriteToken(jwtSecurityToken);
            return jwtToken;
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var userFromDb = await _context.ApplicationUsers.FirstOrDefaultAsync(au => au.Email == registerUserDto.Email);
            if (userFromDb != null)
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
