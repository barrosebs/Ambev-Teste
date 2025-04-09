using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ambev.Domain.Interfaces;
using Ambev.Infrastructure.Data;
using Ambev.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Ambev.Application.Interfaces;
using Ambev.Application.Services;

namespace Ambev.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra o DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Registra os repositórios
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IUserService, UserService>();
            
            return services;
        }
    }
} 