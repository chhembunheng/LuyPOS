namespace LuyPOS.Application.DTOs.Products;

public sealed record CreateProductRequest(
    string Sku,
    string Name,
    string? Description,
    decimal UnitPrice,
    decimal CostPrice,
    int QuantityOnHand,
    int ReorderLevel,
    bool IsActive = true);
