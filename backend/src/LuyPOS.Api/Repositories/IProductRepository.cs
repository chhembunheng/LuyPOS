using LuyPOS.Api.Models;

namespace LuyPOS.Api.Repositories;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetActiveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySkuAsync(string sku, long? excludingProductId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
