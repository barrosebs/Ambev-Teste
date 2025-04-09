using Ambev.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Application.Interfaces
{
    public interface ICartService
    {
        Task<PagedResult<CartDTO>> GetAllAsync(int page = 1, int pageSize = 10, string? order = null, IDictionary<string, string>? filters = null);
        Task<CartDTO?> GetByIdAsync(int id);
        Task<CartDTO> CreateAsync(CartDTO cartDTO);
        Task<CartDTO?> UpdateAsync(int id, CartDTO cartDTO);
        Task<bool> DeleteAsync(int id);
    }
} 