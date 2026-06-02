namespace LuyPOS.Application.DTOs.Products;

public sealed record ProductResponse(
    long Id,
    string Sku,
    string Name,
    string? Description,
    decimal UnitPrice,
    decimal CostPrice,
    int QuantityOnHand,
    int ReorderLevel,
    bool IsActive,
    DateTime? CreatedAt,
    DateTime? UpdatedAt);
