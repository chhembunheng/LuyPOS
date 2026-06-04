using System.ComponentModel.DataAnnotations;

namespace LuyPOS.Api.Dtos.User
{
    public class UserLoginRespone
    {
        public UserLoginRespone(string message, bool success, string username)
        {
            Message = message;
            Success = success;
            Username = username;
            Token = string.Empty;
            ExpiresIn = 7200;
            RefreshToken = string.Empty;
            Roles = [];
            TokenType = "Bearer";
        }

        public string Message { get; }
        public bool Success { get; }
        public string Username { get; }
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IReadOnlyCollection<string> Roles { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public int Status => Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
    }
}
