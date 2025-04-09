using Ambev.Application.DTOs;
using MediatR;

namespace Ambev.Application.Commands
{
    public class CreateCartCommand : IRequest<CartDTO>
    {
        public CartDTO Cart { get; set; } = null!;
    }

    public class UpdateCartCommand : IRequest<CartDTO>
    {
        public int Id { get; set; }
        public CartDTO Cart { get; set; } = null!;
    }

    public class DeleteCartCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
} 