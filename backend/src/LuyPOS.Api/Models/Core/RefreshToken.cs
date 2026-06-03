namespace LuyPOS.Api.Models;

public sealed class RefreshToken
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool Revoked { get; set; }

    public User User { get; set; } = null!;
}
