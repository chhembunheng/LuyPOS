using LuyPOS.Application.Common.Exceptions;
using LuyPOS.Application.DTOs.Products;
using LuyPOS.Application.Interfaces.Services;
using LuyPOS.Application.Mappings;
using LuyPOS.Application.Validators;
using LuyPOS.Domain.Entities;
using LuyPOS.Domain.Interfaces.Repositories;

namespace LuyPOS.Application.Features.Products;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        return products.Select(product => product.ToResponse()).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);
        return product.ToResponse();
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        ProductRequestValidator.Validate(request);

        if (await productRepository.ExistsBySkuAsync(request.Sku, cancellationToken: cancellationToken))
        {
            throw new ValidationException(["Product SKU already exists."]);
        }

        var product = new Product
        {
            Sku = request.Sku.Trim(),
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
        ProductRequestValidator.Validate(request);

        var product = await GetProductOrThrowAsync(id, cancellationToken);

        if (await productRepository.ExistsBySkuAsync(request.Sku, id, cancellationToken))
        {
            throw new ValidationException(["Product SKU already exists."]);
        }

        product.Sku = request.Sku.Trim();
        product.Name = request.Name.Trim();
        product.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        product.UnitPrice = request.UnitPrice;
        product.CostPrice = request.CostPrice;
        product.QuantityOnHand = request.QuantityOnHand;
        product.ReorderLevel = request.ReorderLevel;
        product.IsActive = request.IsActive;

        productRepository.Update(product);
        await productRepository.SaveChangesAsync(cancellationToken);

        return product.ToResponse();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);

        productRepository.Delete(product);
        await productRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> GetProductOrThrowAsync(long id, CancellationToken cancellationToken)
    {
        return await productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Product with ID '{id}' was not found.");
    }
}
