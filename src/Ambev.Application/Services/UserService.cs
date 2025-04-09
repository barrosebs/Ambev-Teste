using Ambev.Application.DTOs;
using Ambev.Application.Interfaces;
using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace Ambev.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDTO>> GetUsersAsync(int page = 1, int pageSize = 10, string? order = null, Dictionary<string, string>? filters = null)
        {
            var users = await _userRepository.GetPagedFilteredAndSortedAsync(page, pageSize, order, filters);
            var totalItems = await _userRepository.GetFilteredCountAsync(filters);

            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return new PagedResult<UserDTO>
            {
                Data = userDtos,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDto)
        {
            if (await _userRepository.GetByEmailAsync(userDto.Email) != null)
            {
                throw new ArgumentException("Email já cadastrado.", nameof(userDto.Email));
            }
            if (await _userRepository.GetByUsernameAsync(userDto.Username) != null)
            {
                throw new ArgumentException("Nome de usuário já cadastrado.", nameof(userDto.Username));
            }

            var user = _mapper.Map<User>(userDto);

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }
            else
            {
                throw new ArgumentException("A senha é obrigatória para criar usuário.", nameof(userDto.Password));
            }

            user.CreatedAt = DateTime.UtcNow;
            user.Status = "Active";

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> UpdateUserAsync(int id, UserDTO userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return null;
            }

            if (existingUser.Email != userDto.Email && await _userRepository.GetByEmailAsync(userDto.Email) != null)
            {
                throw new ArgumentException("Email já cadastrado.", nameof(userDto.Email));
            }
            if (existingUser.Username != userDto.Username && await _userRepository.GetByUsernameAsync(userDto.Username) != null)
            {
                throw new ArgumentException("Nome de usuário já cadastrado.", nameof(userDto.Username));
            }

            _mapper.Map(userDto, existingUser);

            if (!string.IsNullOrEmpty(userDto.Password))
            {

                // Replace the line causing the error with the correct namespace and method
                if (!string.IsNullOrEmpty(userDto.Password))
                {
                    existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                }
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            existingUser.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(existingUser);

            return _mapper.Map<UserDTO>(existingUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public Task<UserDTO> FindAsync(Expression<Func<UserDTO, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
} 