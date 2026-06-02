using LuyPOS.Application.DTOs.Products;
using LuyPOS.Domain.Entities;

namespace LuyPOS.Application.Mappings;

public static class ProductMapping
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Sku,
            product.Name,
            product.Description,
            product.UnitPrice,
            product.CostPrice,
            product.QuantityOnHand,
            product.ReorderLevel,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);
    }
}
