using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LuyPOS.Api.Dtos.User;
using LuyPOS.Api.Models;
using LuyPOS.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LuyPOS.Api.Services;

public sealed class AuthService(
    IAuthRepository authRepository,
    IConfiguration configuration) : IAuthService
{
    private readonly PasswordHasher<User> passwordHasher = new();

    public async Task<RegisterResponse> RegisterAsync(
        UserRegisterDto request,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        ValidateRegister(request);

        var username = request.Username.Trim();
        var email = request.Email.Trim();

        if (await authRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new RequestValidationException(["Email is already registered."]);
        }

        if (await authRepository.ExistsByUsernameAsync(username, cancellationToken))
        {
            throw new RequestValidationException(["Username is already registered."]);
        }

        var user = new User
        {
            Name = username,
            Email = email
        };
        user.Password = passwordHasher.HashPassword(user, request.Password);

        await authRepository.AddUserAsync(user, cancellationToken);
        await authRepository.SaveChangesAsync(cancellationToken);

        var refreshToken = GenerateRefreshToken(user);
        await authRepository.AddRefreshTokenAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);
        await authRepository.AddLoginHistoryAsync(new LoginHistory
        {
            UserId = user.Id,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            LoginAt = DateTime.UtcNow
        }, cancellationToken);
        await authRepository.SaveChangesAsync(cancellationToken);

        var roles = await authRepository.GetRoleNamesAsync(user.Id, cancellationToken);

        return new RegisterResponse(
            "User registered successfully.",
            true,
            user.Name,
            user.Id,
            GenerateAccessToken(user, roles),
            refreshToken,
            roles);
    }

    public async Task<UserLoginRespone> LoginAsync(
        UserLoginDto request,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        ValidateLogin(request);

        var username = request.Username.Trim();
        var user = await authRepository.GetByUsernameOrEmailAsync(username, cancellationToken);

        if (user is null || !user.Enabled)
        {
            throw new RequestValidationException(["Invalid username or password."]);
        }

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new RequestValidationException(["Invalid username or password."]);
        }

        var refreshToken = GenerateRefreshToken(user);
        await authRepository.AddRefreshTokenAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);
        await authRepository.AddLoginHistoryAsync(new LoginHistory
        {
            UserId = user.Id,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            LoginAt = DateTime.UtcNow
        }, cancellationToken);
        await authRepository.SaveChangesAsync(cancellationToken);

        var roles = await authRepository.GetRoleNamesAsync(user.Id, cancellationToken);

        return new UserLoginRespone("Login successful.", true, user.Name)
        {
            Token = GenerateAccessToken(user, roles),
            RefreshToken = refreshToken,
            Roles = roles,
            ExpiresIn = 7200
        };
    }

    private string GenerateAccessToken(User user, IReadOnlyCollection<string> roles)
    {
        return GenerateJwtToken(user, roles, DateTime.UtcNow.AddHours(2), "access");
    }

    private string GenerateRefreshToken(User user)
    {
        return GenerateJwtToken(user, [], DateTime.UtcNow.AddDays(7), "refresh");
    }

    private string GenerateJwtToken(
        User user,
        IReadOnlyCollection<string> roles,
        DateTime expiresAt,
        string tokenType)
    {
        var claims = new List<Claim>
        {
            new("user_id", user.Id.ToString()),
            new("username", user.Name),
            new("email", user.Email),
            new("token_type", tokenType),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim("role", role)));

        var jwtSecretKey = configuration.GetValue<string>("AppSettings:JwtSecretKey")
            ?? throw new InvalidOperationException("AppSettings:JwtSecretKey is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void ValidateRegister(UserRegisterDto request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            errors.Add("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errors.Add("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add("Password is required.");
        }

        if (request.Password != request.ConfirmPassword)
        {
            errors.Add("Passwords do not match.");
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }
    }

    private static void ValidateLogin(UserLoginDto request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            errors.Add("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add("Password is required.");
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }
    }
}
