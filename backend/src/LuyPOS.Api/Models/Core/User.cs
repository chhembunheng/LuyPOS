namespace LuyPOS.Api.Models;

public sealed class User
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Enabled { get; set; } = true;
    public bool EmailVerified { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }

    public UserProfile? Profile { get; set; }
    public ICollection<UserTranslation> Translations { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<UserPermission> UserPermissions { get; set; } = [];
    public ICollection<UserSession> Sessions { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<UserOtp> Otps { get; set; } = [];
    public ICollection<UserForgotPassword> ForgotPasswordTokens { get; set; } = [];
    public ICollection<LoginHistory> LoginHistories { get; set; } = [];
    public ICollection<AuditLog> AuditLogs { get; set; } = [];
    public ICollection<UserActivity> Activities { get; set; } = [];
}
