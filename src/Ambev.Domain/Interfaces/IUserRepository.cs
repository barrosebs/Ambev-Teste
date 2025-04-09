using Ambev.Domain.Entities;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Domain.Interfaces
{
    // IUserRepository agora herda os métodos CRUD básicos de IRepository<User>
    public interface IUserRepository : IRepository<User>
    {
        // Métodos específicos de User
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);

        // Métodos para busca avançada
        Task<IEnumerable<User>> GetPagedFilteredAndSortedAsync(int page, int pageSize, string? order = null, Dictionary<string, string>? filters = null);
        Task<int> GetFilteredCountAsync(Dictionary<string, string>? filters = null);
        Task<int> GetTotalCountAsync();

        // Métodos CRUD (FindAsync, AddAsync, UpdateAsync, DeleteAsync) herdados de IRepository<User>
        // Não precisam ser redeclarados aqui.
    }
} 