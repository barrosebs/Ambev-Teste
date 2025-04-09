using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using Ambev.Infrastructure.Data;
using Ambev.Infrastructure.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Infrastructure.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Cart>> GetByUserIdAsync(int userId)
        {
            return await _dbSet.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetPagedFilteredAndSortedAsync(
            int page, int pageSize, string? order = null, IDictionary<string, string>? filters = null)
        {
            IQueryable<Cart> query = _dbSet;

            if (filters != null)
            {
                query = query.ApplyFilters(filters.ToDictionary(x => x.Key, x => x.Value));
            }

            query = query.ApplySorting(order);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetFilteredCountAsync(IDictionary<string, string>? filters = null)
        {
            IQueryable<Cart> query = _dbSet;

            if (filters != null)
            {
                query = query.ApplyFilters(filters.ToDictionary(x => x.Key, x => x.Value));
            }

            return await query.CountAsync();
        }
    }
} 