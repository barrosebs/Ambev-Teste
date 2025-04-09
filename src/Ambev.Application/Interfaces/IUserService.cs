using Ambev.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Ambev.Application.Interfaces
{
    public interface IUserService
    {
        // Get all users with pagination, filtering, and sorting
        // TODO: Add parameters for filtering and sorting based on documentation
        Task<PagedResult<UserDTO>> GetUsersAsync(int page = 1, int pageSize = 10, string? order = null, Dictionary<string, string>? filters = null);

        Task<UserDTO?> GetUserByIdAsync(int id);

        // Use a specific DTO for creation to handle password properly
        Task<UserDTO> CreateUserAsync(UserDTO userDto);

        // Use a specific DTO for update
        Task<UserDTO?> UpdateUserAsync(int id, UserDTO userDto);

        Task<bool> DeleteUserAsync(int id);

        Task<UserDTO> FindAsync(Expression<Func<UserDTO, bool>> predicate);
    }
} 