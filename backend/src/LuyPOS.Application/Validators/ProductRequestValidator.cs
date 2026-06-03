using LuyPOS.Application.Common.Exceptions;
using LuyPOS.Application.DTOs.Products;

namespace LuyPOS.Application.Validators;

public static class ProductRequestValidator
{
    public static void Validate(CreateProductRequest request)
    {
        ValidateCore(request.Sku, request.Name, request.UnitPrice, request.CostPrice, request.QuantityOnHand, request.ReorderLevel);
    }

    public static void Validate(UpdateProductRequest request)
    {
        ValidateCore(request.Sku, request.Name, request.UnitPrice, request.CostPrice, request.QuantityOnHand, request.ReorderLevel);
    }

    private static void ValidateCore(string sku, string name, decimal unitPrice, decimal costPrice, int quantityOnHand, int reorderLevel)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(sku))
        {
            errors.Add("Product SKU is required.");
        }
        else if (sku.Length > 64)
        {
            errors.Add("Product SKU must be 64 characters or fewer.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("Product name is required.");
        }
        else if (name.Length > 160)
        {
            errors.Add("Product name must be 160 characters or fewer.");
        }

        if (unitPrice < 0)
        {
            errors.Add("Unit price cannot be negative.");
        }

        if (costPrice < 0)
        {
            errors.Add("Cost price cannot be negative.");
        }

        if (quantityOnHand < 0)
        {
            errors.Add("Quantity on hand cannot be negative.");
        }

        if (reorderLevel < 0)
        {
            errors.Add("Reorder level cannot be negative.");
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}
