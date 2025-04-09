using Ambev.Application.Commands;
using Ambev.Application.DTOs;
using Ambev.Application.Mappings;
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
    public class ProductCommandHandlers :
        IRequestHandler<CreateProductCommand, ProductDTO>,
        IRequestHandler<UpdateProductCommand, ProductDTO?>,
        IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductCommandHandlers(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (request?.Product == null)
                throw new ArgumentNullException(nameof(request.Product));

            var product = _mapper.Map<Product>(request.Product);
            product.CreatedAt = DateTime.UtcNow;
            
            await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (request?.Product == null)
                throw new ArgumentNullException(nameof(request.Product));

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                return null;

            _mapper.Map(request.Product, product);
            product.UpdatedAt = DateTime.UtcNow;
            
            await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }
    }

    public class ProductQueryHandlers :
        IRequestHandler<GetProductByIdQuery, ProductDTO?>,
        IRequestHandler<GetAllProductsQuery, PagedResult<ProductDTO>>,
        IRequestHandler<GetProductCategoriesQuery, IEnumerable<string>>,
        IRequestHandler<GetProductsByCategoryQuery, IEnumerable<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductQueryHandlers(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            return product != null ? _mapper.Map<ProductDTO>(product) : null;
        }

        public async Task<PagedResult<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetPagedFilteredAndSortedAsync(request.Page, request.Size, request.Order, request.Filters);
            var totalItems = await _productRepository.GetFilteredCountAsync(request.Filters);
            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return new PagedResult<ProductDTO>
            {
                Data = productDtos,
                CurrentPage = request.Page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.Size)
            };
        }

        public async Task<IEnumerable<string>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllCategoriesAsync() ?? new List<string>();
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Category))
                throw new ArgumentException("Categoria não pode ser nula ou vazia.", nameof(request.Category));

            var products = await _productRepository.GetByCategoryAsync(request.Category);
            return _mapper.Map<IEnumerable<ProductDTO>>(products ?? new List<Product>());
        }
    }
} 