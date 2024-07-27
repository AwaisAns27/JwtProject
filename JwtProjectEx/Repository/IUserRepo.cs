using JwtProjectEx.ApiResponses;
using JwtProjectEx.Dtos;

namespace JwtProjectEx.Repository
{
    public interface IUserRepo
    {
        public Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto registerUserDto);
        public Task<LogInUserResponse> LogInUserAsync(LogInUserDto logInUserDto);

    }
}
