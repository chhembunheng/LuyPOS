using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LuyPOS.Api.Dtos.User;
using LuyPOS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LuyPOS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RegisterResponse> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Name = userRegisterDto.Username,
            Email = userRegisterDto.Email
        };
        user.Password = new PasswordHasher<User>().HashPassword(user, userRegisterDto.Password);

        return Ok(new RegisterResponse("User registered successfully.", true, user.Name));
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = new User
        {
            Name = userLoginDto.Username,
            Password = new PasswordHasher<User>().HashPassword(null, userLoginDto.Password)
        };
        var token = GenerateJwtToken(user);
        return Ok(new UserLoginRespone("Login successful.", true, user.Name) { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var jwtSecretKey = configuration.GetValue<string>("AppSettings:JwtSecretKey")
            ?? throw new InvalidOperationException("AppSettings:JwtSecretKey is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
