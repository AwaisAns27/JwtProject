using System.ComponentModel.DataAnnotations;

namespace JwtProjectEx.Dtos
{
    public class LogInUserDto
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
