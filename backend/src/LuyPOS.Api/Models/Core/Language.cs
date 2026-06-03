namespace LuyPOS.Api.Models;

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
