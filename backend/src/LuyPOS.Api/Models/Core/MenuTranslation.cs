namespace LuyPOS.Api.Models;

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
