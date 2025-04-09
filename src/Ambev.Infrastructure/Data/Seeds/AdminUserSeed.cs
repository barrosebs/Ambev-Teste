using Ambev.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ambev.Infrastructure.Data.Seeds
{
    public static class AdminUserSeed
    {
        public static async Task SeedAdminUser(ApplicationDbContext context)
        {
            if (!await context.Users.AnyAsync(u => u.Role == "Admin"))
            {
                var adminUser = new User
                {
                    Email = "admin@ambev.com",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Firstname = "Administrador",
                    Lastname = "Sistema",
                    City = "São Paulo",
                    Street = "Avenida Paulista",
                    Number = "1000",
                    Zipcode = "01310-100",
                    GeolocationLat = "-23.5630",
                    GeolocationLong = "-46.6543",
                    Phone = "(11) 99999-9999",
                    Status = "Active",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
} 