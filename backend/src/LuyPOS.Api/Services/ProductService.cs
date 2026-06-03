using LuyPOS.Api.Data;
using LuyPOS.Api.Dtos;
using LuyPOS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Api.Services;

public sealed class ProductService(LuyPosDbContext dbContext)
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(product => product.DeletedAt == null)
            .OrderBy(product => product.Name)
            .Select(product => new ProductResponse(
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
                product.UpdatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);
        return product.ToResponse();
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);
        var sku = request.Sku.Trim();

        if (await ExistsBySkuAsync(sku, cancellationToken: cancellationToken))
        {
            throw new RequestValidationException(["Product SKU already exists."]);
        }

        var product = new Product
        {
            Sku = sku,
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            UnitPrice = request.UnitPrice,
            CostPrice = request.CostPrice,
            QuantityOnHand = request.QuantityOnHand,
            ReorderLevel = request.ReorderLevel,
            IsActive = request.IsActive
        };

        await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product.ToResponse();
    }

    public async Task<ProductResponse> UpdateAsync(long id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);
        var sku = request.Sku.Trim();

        var product = await GetProductOrThrowAsync(id, cancellationToken);

        if (await ExistsBySkuAsync(sku, id, cancellationToken))
        {
            throw new RequestValidationException(["Product SKU already exists."]);
        }

        product.Sku = sku;
        product.Name = request.Name.Trim();
        product.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        product.UnitPrice = request.UnitPrice;
        product.CostPrice = request.CostPrice;
        product.QuantityOnHand = request.QuantityOnHand;
        product.ReorderLevel = request.ReorderLevel;
        product.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return product.ToResponse();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);
        product.DeletedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> GetProductOrThrowAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id && product.DeletedAt == null, cancellationToken)
            ?? throw new NotFoundException($"Product with ID '{id}' was not found.");
    }

    private async Task<bool> ExistsBySkuAsync(string sku, long? excludingProductId = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.AnyAsync(
            product => product.Sku == sku
                && product.DeletedAt == null
                && (!excludingProductId.HasValue || product.Id != excludingProductId.Value),
            cancellationToken);
    }

    private static void Validate(CreateProductRequest request)
    {
        ValidateProduct(request.Sku, request.Name, request.UnitPrice, request.CostPrice, request.QuantityOnHand, request.ReorderLevel);
    }

    private static void Validate(UpdateProductRequest request)
    {
        ValidateProduct(request.Sku, request.Name, request.UnitPrice, request.CostPrice, request.QuantityOnHand, request.ReorderLevel);
    }

    private static void ValidateProduct(string sku, string name, decimal unitPrice, decimal costPrice, int quantityOnHand, int reorderLevel)
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
            throw new RequestValidationException(errors);
        }
    }
}

file static class ProductMapping
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
