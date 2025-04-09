using Ambev.Application.DTOs;
using Ambev.Application.Interfaces;
using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq.Expressions;
using Ambev.Domain.DTOs;

namespace Ambev.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.FindAsync(u => u.Email == email);

            if (user == null || !user.Any())
                return null;

            var userEntity = user.First();
            return new LoginResponseDTO
            {
                Email = userEntity.Email,
                Username = userEntity.Username,
                Role = userEntity.Role,
                Token = string.Empty, 
                Expiration = DateTime.UtcNow.AddHours(1) 
            };
        }

        public async Task<LoginResponseDTO?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.FindAsync(u => u.Email == email);
            if (user == null || !user.Any())
                return null; 

            var userEntity = user.First();

            if (!VerifyPassword(password, userEntity.PasswordHash))
                return null; 

            var token = GenerateJwtToken(userEntity);

            return new LoginResponseDTO
            {
                Token = token,
                Email = userEntity.Email,
                Username = userEntity.Username,
                Role = userEntity.Role,
                Expiration = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<bool> RegisterAsync(UserDTO userDTO)
        {
            Expression<Func<User, bool>> predicate = u => u.Email == userDTO.Email;
            var existingUser = await _userRepository.FindAsync(predicate);
            if (existingUser != null && existingUser.Any())
                return false;

            var user = _mapper.Map<User>(userDTO);
            await _userRepository.AddAsync(user);
            return true;
        }

        public Task<bool> RegisterAsync(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
} 