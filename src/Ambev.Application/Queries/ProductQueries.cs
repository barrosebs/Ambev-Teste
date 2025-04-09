using Ambev.Application.DTOs;
using Ambev.Application.Mappings;
using MediatR;
using System.Collections.Generic;

namespace Ambev.Application.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductDTO?>;

    public record GetAllProductsQuery(
        int Page = 1,
        int Size = 10,
        string? Order = null,
        IDictionary<string, string>? Filters = null) : IRequest<PagedResult<ProductDTO>>;

    public record GetProductCategoriesQuery() : IRequest<IEnumerable<string>>;

    public record GetProductsByCategoryQuery(string Category) : IRequest<IEnumerable<ProductDTO>>;
} 