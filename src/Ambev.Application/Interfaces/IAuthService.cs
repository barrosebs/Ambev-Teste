using Ambev.Domain.DTOs;

namespace Ambev.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(LoginDTO loginDTO);
        Task<LoginResponseDTO?> GetByEmailAsync(string email);
    }
} 