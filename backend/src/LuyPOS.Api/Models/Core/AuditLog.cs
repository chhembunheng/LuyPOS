namespace LuyPOS.Api.Models;

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
