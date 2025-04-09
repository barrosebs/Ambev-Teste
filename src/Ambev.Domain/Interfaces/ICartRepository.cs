using Ambev.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<IEnumerable<Cart>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Cart>> GetPagedFilteredAndSortedAsync(int page, int pageSize, string? order = null, IDictionary<string, string>? filters = null);
        Task<int> GetFilteredCountAsync(IDictionary<string, string>? filters = null);
    }
} 