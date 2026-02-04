using BusinessWeb.Application.Interfaces;
using BusinessWeb.Infrastructure.Data;
using BusinessWeb.Infrastructure.Repositories;
using BusinessWeb.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusinessWeb.Application.Interfaces.Products;
using BusinessWeb.Application.Interfaces.Sales;

namespace BusinessWeb.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            var cs = config.GetConnectionString("Default");
            opt.UseNpgsql(cs);
        });

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISaleService, SaleService>();

        return services;
    }
}
