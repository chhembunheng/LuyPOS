namespace LuyPOS.Domain.Entities;

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

public sealed class UserProfile
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
}

public sealed class UserTranslation
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long LanguageId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public Language Language { get; set; } = null!;
}

public sealed class Language
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NativeName { get; set; }
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; }
    public string Direction { get; set; } = "ltr";
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? FlagImage { get; set; }

    public ICollection<UserTranslation> UserTranslations { get; set; } = [];
    public ICollection<RoleTranslation> RoleTranslations { get; set; } = [];
    public ICollection<PermissionTranslation> PermissionTranslations { get; set; } = [];
    public ICollection<MenuTranslation> MenuTranslations { get; set; } = [];
}

public sealed class Role
{
    public long Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<RoleTranslation> Translations { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}

public sealed class RoleTranslation
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Role Role { get; set; } = null!;
    public Language Language { get; set; } = null!;
}

public sealed class Permission
{
    public long Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public bool IsMenu { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<PermissionTranslation> Translations { get; set; } = [];
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
    public ICollection<UserPermission> UserPermissions { get; set; } = [];
    public ICollection<MenuPermission> MenuPermissions { get; set; } = [];
}

public sealed class PermissionTranslation
{
    public long Id { get; set; }
    public long PermissionId { get; set; }
    public long LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Permission Permission { get; set; } = null!;
    public Language Language { get; set; } = null!;
}

public sealed class Menu
{
    public long Id { get; set; }
    public long? ParentId { get; set; }
    public string? Icon { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = [];
    public ICollection<MenuTranslation> Translations { get; set; } = [];
    public ICollection<MenuPermission> MenuPermissions { get; set; } = [];
}

public sealed class MenuTranslation
{
    public long Id { get; set; }
    public long MenuId { get; set; }
    public long LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Menu Menu { get; set; } = null!;
    public Language Language { get; set; } = null!;
}

public sealed class UserRole
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}

public sealed class UserPermission
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long PermissionId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}

public sealed class RolePermission
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}

public sealed class MenuPermission
{
    public long Id { get; set; }
    public long MenuId { get; set; }
    public long PermissionId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Menu Menu { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}

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

public sealed class UserForgotPassword
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
}

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

public sealed class AuditLog
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Browser { get; set; }
    public string? Platform { get; set; }
    public DateTime? CreatedAt { get; set; }

    public User? User { get; set; }
    public ICollection<AuditLogDetail> Details { get; set; } = [];
}

public sealed class AuditLogDetail
{
    public long Id { get; set; }
    public long AuditLogId { get; set; }
    public string KeyName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime? CreatedAt { get; set; }

    public AuditLog AuditLog { get; set; } = null!;
}

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
