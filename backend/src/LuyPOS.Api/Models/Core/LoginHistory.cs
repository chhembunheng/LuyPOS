namespace LuyPOS.Api.Models;

public sealed class LoginHistory
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Browser { get; set; }
    public string? Platform { get; set; }
    public DateTime? LoginAt { get; set; }
    public DateTime? LogoutAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
}
