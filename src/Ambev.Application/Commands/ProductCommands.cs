using Ambev.Application.DTOs;
using MediatR;

namespace Ambev.Application.Commands
{
    public record CreateProductCommand(ProductDTO Product) : IRequest<ProductDTO>;

    public record UpdateProductCommand(int Id, ProductDTO Product) : IRequest<ProductDTO?>;

    public record DeleteProductCommand(int Id) : IRequest<bool>;
} 