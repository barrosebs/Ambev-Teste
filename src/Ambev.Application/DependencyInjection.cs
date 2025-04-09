using Microsoft.Extensions.DependencyInjection;
using Ambev.Application.Interfaces;
using Ambev.Application.Services;
using Ambev.Application.Mappings;
using AutoMapper;

namespace Ambev.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registra os serviços
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            // Registra o AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
} 