using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using Ambev.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Ambev.Infrastructure.Repositories.Extensions;
using System;

namespace Ambev.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetPagedFilteredAndSortedAsync(
            int page, int pageSize, string? order = null, Dictionary<string, string>? filters = null)
        {
            IQueryable<User> query = _dbSet;

            // Aplicar Filtros e Ordenação usando extensões
            query = query.ApplyFilters(filters)
                         .ApplySorting(order);

            // Aplicar Paginação
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetFilteredCountAsync(Dictionary<string, string>? filters = null)
        {
            IQueryable<User> query = _dbSet;
            query = query.ApplyFilters(filters);
            return await query.CountAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException($"Entidade do tipo {typeof(User).Name} com ID {id} não encontrada");
        }

        public override async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.Where(predicate).ToListAsync();
        }

        public override async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
} 