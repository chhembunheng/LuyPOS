namespace LuyPOS.Api.Models;

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
