namespace LuyPOS.Api.Models;

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
