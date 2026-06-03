namespace LuyPOS.Api.Models;

public sealed class UserOtp
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string OtpCode { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? Purpose { get; set; }

    public User User { get; set; } = null!;
}
