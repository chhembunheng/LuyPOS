namespace LuyPOS.Api.Dtos;

public sealed record UpdateProductRequest(
    string Sku,
    string Name,
    string? Description,
    decimal UnitPrice,
    decimal CostPrice,
    int QuantityOnHand,
    int ReorderLevel,
    bool IsActive);
