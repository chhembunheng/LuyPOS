namespace LuyPOS.Api.Models;

public sealed class UserTranslation
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long LanguageId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public Language Language { get; set; } = null!;
}
