namespace LuyPOS.Api.Dtos.User;

public sealed class RegisterResponse
{
    public RegisterResponse(string message, bool success, string username)
    {
        Message = message;
        Success = success;
        Username = username;
    }

    public string Message { get; }
    public bool Success { get; }
    public string Username { get; }
    public int Status => Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
}
