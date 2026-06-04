using LuyPOS.Api.Dtos.User;

namespace LuyPOS.Api.Services;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(
        UserRegisterDto request,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task<UserLoginRespone> LoginAsync(
        UserLoginDto request,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}
