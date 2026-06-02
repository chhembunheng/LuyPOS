using LuyPOS.Domain.Entities;
using LuyPOS.Domain.Interfaces.Repositories;
using LuyPOS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Infrastructure.Repositories;

public sealed class ProductRepository(LuyPosDbContext dbContext) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(product => product.DeletedAt == null)
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id && product.DeletedAt == null, cancellationToken);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Sku == sku && product.DeletedAt == null, cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
    }

    public void Update(Product product)
    {
        dbContext.Products.Update(product);
    }

    public void Delete(Product product)
    {
        product.DeletedAt = DateTime.UtcNow;
        dbContext.Products.Update(product);
    }

    public async Task<bool> ExistsBySkuAsync(string sku, long? excludingProductId = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.AnyAsync(
            product => product.Sku == sku
                && product.DeletedAt == null
                && (!excludingProductId.HasValue || product.Id != excludingProductId.Value),
            cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
