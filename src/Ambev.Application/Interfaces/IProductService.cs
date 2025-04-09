using Ambev.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetProductsAsync(int page = 1, int pageSize = 10, string? order = null, IDictionary<string, string>? filters = null);
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(ProductDTO productDto);
        Task<ProductDTO?> UpdateProductAsync(int id, ProductDTO productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
    }
} 