namespace LuyPOS.Api.Models;

public sealed class UserSession
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
}
