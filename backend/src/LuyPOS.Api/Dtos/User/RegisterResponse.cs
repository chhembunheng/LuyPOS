namespace LuyPOS.Api.Dtos.User;

public sealed class RegisterResponse
{
    public RegisterResponse(
        string message,
        bool success,
        string username,
        long id,
        string accessToken = "",
        string refreshToken = "",
        IReadOnlyCollection<string>? roles = null)
    {
        Message = message;
        Success = success;
        Username = username;
        Id = id;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Roles = roles ?? [];
        TokenType = "Bearer";
        Timestamp = DateTime.UtcNow;
    }

    public string Message { get; }
    public bool Success { get; }
    public string Username { get; }
    public long Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public IReadOnlyCollection<string> Roles { get; set; }
    public string TokenType { get; set; }
    public DateTime Timestamp { get; }
}
