using LuyPOS.Application.Features.Products;
using LuyPOS.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LuyPOS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
