using Ambev.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<IEnumerable<Product>> GetPagedFilteredAndSortedAsync(int page, int pageSize, string? order = null, IDictionary<string, string>? filters = null);
        Task<int> GetFilteredCountAsync(IDictionary<string, string>? filters = null);
    }
} 