using LuyPOS.Api.Dtos;
using LuyPOS.Api.Models;
using LuyPOS.Api.Repositories;

namespace LuyPOS.Api.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetAllActiveAsync(cancellationToken);
        return products.Select(product => product.ToResponse()).ToList();
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

        if (await productRepository.ExistsBySkuAsync(sku, cancellationToken: cancellationToken))
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

        await productRepository.AddAsync(product, cancellationToken);
        await productRepository.SaveChangesAsync(cancellationToken);

        return product.ToResponse();
    }

    public async Task<ProductResponse> UpdateAsync(long id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);
        var sku = request.Sku.Trim();

        var product = await GetProductOrThrowAsync(id, cancellationToken);

        if (await productRepository.ExistsBySkuAsync(sku, id, cancellationToken))
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

        await productRepository.SaveChangesAsync(cancellationToken);

        return product.ToResponse();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);
        product.DeletedAt = DateTime.UtcNow;
        await productRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> GetProductOrThrowAsync(long id, CancellationToken cancellationToken)
    {
        return await productRepository.GetActiveByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Product with ID '{id}' was not found.");
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
            product.FrontendGuid,
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
