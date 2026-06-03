namespace LuyPOS.Api.Models;

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
