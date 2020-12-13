using System.ComponentModel.DataAnnotations;

namespace JwtAuthDemo.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public string Token { get; set; }
    }

    public class RefreshTokenViewModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}