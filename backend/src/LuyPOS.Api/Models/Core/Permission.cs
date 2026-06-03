namespace LuyPOS.Api.Models;

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
