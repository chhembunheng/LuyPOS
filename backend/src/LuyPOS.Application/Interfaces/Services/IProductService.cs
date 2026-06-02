using LuyPOS.Application.DTOs.Products;

namespace LuyPOS.Application.Interfaces.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<ProductResponse> UpdateAsync(long id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
