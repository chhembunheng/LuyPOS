using LuyPOS.Api.Data;
using LuyPOS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Api.Repositories;

public sealed class ProductRepository(LuyPosDbContext dbContext) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(product => product.DeletedAt == null)
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetActiveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id && product.DeletedAt == null, cancellationToken);
    }

    public async Task<bool> ExistsBySkuAsync(
        string sku,
        long? excludingProductId = null,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.AnyAsync(
            product => product.Sku == sku
                && product.DeletedAt == null
                && (!excludingProductId.HasValue || product.Id != excludingProductId.Value),
            cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
