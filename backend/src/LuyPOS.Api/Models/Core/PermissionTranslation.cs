namespace LuyPOS.Api.Models;

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
