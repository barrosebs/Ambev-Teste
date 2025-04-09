using Ambev.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Ambev.Application.Queries
{
    public class GetUserByIdQuery : IRequest<UserDTO?>
    {
        public int Id { get; set; }
    }

    public class GetAllUsersQuery : IRequest<PagedResult<UserDTO>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Order { get; set; }
        public Dictionary<string, string>? Filters { get; set; }
    }
} 