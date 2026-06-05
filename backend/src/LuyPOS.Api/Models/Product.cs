namespace LuyPOS.Api.Models;

public sealed class Product : AuditableEntity
{
    public Guid FrontendGuid { get; set; } = Guid.NewGuid();
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal CostPrice { get; set; }
    public int QuantityOnHand { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; } = true;
}
