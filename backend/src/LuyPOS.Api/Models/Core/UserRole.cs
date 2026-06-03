namespace LuyPOS.Api.Models;

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
