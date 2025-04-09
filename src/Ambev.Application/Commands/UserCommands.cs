using Ambev.Application.DTOs;
using MediatR;

namespace Ambev.Application.Commands
{
    public class CreateUserCommand : IRequest<UserDTO>
    {
        public UserDTO User { get; set; } = null!;
    }

    public class UpdateUserCommand : IRequest<UserDTO>
    {
        public int Id { get; set; }
        public UserDTO User { get; set; } = null!;
    }

    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
} 