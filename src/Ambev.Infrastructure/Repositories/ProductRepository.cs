using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using Ambev.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.Infrastructure.Repositories.Extensions;
using System.Linq.Expressions;
using System;

namespace Ambev.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetPagedFilteredAndSortedAsync(
            int page, int pageSize, string? order = null, IDictionary<string, string>? filters = null)
        {
            IQueryable<Product> query = _dbSet;

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
            IQueryable<Product> query = _dbSet;

            if (filters != null)
            {
                query = query.ApplyFilters(filters.ToDictionary(x => x.Key, x => x.Value));
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _dbSet.Select(p => p.Category).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            var filters = new Dictionary<string, string> { { nameof(Product.Category), category } };
            IQueryable<Product> query = _dbSet.ApplyFilters(filters);
            return await query.ToListAsync();
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"Entidade do tipo {typeof(Product).Name} com ID {id} não encontrada");
        }

        public override async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
} 