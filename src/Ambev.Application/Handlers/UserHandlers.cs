using Ambev.Application.Commands;
using Ambev.Application.DTOs;
using Ambev.Application.Queries;
using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.Application.Handlers
{
    public class UserCommandHandlers :
        IRequestHandler<CreateUserCommand, UserDTO>,
        IRequestHandler<UpdateUserCommand, UserDTO?>,
        IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserCommandHandlers(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request?.User == null)
                throw new ArgumentNullException(nameof(request.User));

            var existingUser = await _userRepository.GetByEmailAsync(request.User.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email já está em uso.");

            existingUser = await _userRepository.GetByUsernameAsync(request.User.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Nome de usuário já está em uso.");

            var user = _mapper.Map<User>(request.User);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.User.Password);
            user.CreatedAt = DateTime.UtcNow;
            
            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request?.User == null)
                throw new ArgumentNullException(nameof(request.User));

            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(request.User.Email) && request.User.Email != user.Email)
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.User.Email);
                if (existingUser != null)
                    throw new InvalidOperationException("Email já está em uso.");
            }

            if (!string.IsNullOrEmpty(request.User.Username) && request.User.Username != user.Username)
            {
                var existingUser = await _userRepository.GetByUsernameAsync(request.User.Username);
                if (existingUser != null)
                    throw new InvalidOperationException("Nome de usuário já está em uso.");
            }

            _mapper.Map(request.User, user);
            if (!string.IsNullOrEmpty(request.User.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.User.Password);
            }
            user.UpdatedAt = DateTime.UtcNow;
            
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }
    }

    public class UserQueryHandlers :
        IRequestHandler<GetUserByIdQuery, UserDTO?>,
        IRequestHandler<GetAllUsersQuery, PagedResult<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserQueryHandlers(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDTO?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            return user != null ? _mapper.Map<UserDTO>(user) : null;
        }

        public async Task<PagedResult<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetPagedFilteredAndSortedAsync(request.Page, request.Size, request.Order, request.Filters);
            var totalItems = await _userRepository.GetFilteredCountAsync(request.Filters);
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return new PagedResult<UserDTO>
            {
                Data = userDtos,
                CurrentPage = request.Page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.Size)
            };
        }
    }
} 