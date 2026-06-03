namespace LuyPOS.Api.Models;

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
