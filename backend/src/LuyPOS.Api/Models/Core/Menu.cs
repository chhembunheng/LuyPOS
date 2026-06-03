namespace LuyPOS.Api.Models;

public sealed class Menu
{
    public long Id { get; set; }
    public long? ParentId { get; set; }
    public string? Icon { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = [];
    public ICollection<MenuTranslation> Translations { get; set; } = [];
    public ICollection<MenuPermission> MenuPermissions { get; set; } = [];
}
