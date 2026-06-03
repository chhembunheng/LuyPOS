namespace LuyPOS.Api.Models;

public sealed class UserActivity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? Location { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? DeviceInfo { get; set; }
    public string? BrowserInfo { get; set; }
    public string? CurrentPage { get; set; }
    public DateTime LastActivityAt { get; set; }
    public DateTime ConnectedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DisconnectedAt { get; set; }

    public User User { get; set; } = null!;
}
