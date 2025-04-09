using Ambev.Application.DTOs;
using Ambev.Application.Interfaces;
using Ambev.Application.Mappings;
using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Ambev.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResult<ProductDTO>> GetProductsAsync(int page = 1, int pageSize = 10, string? order = null, IDictionary<string, string>? filters = null)
        {
            var products = await _productRepository.GetPagedFilteredAndSortedAsync(page, pageSize, order, filters);
            var totalItems = await _productRepository.GetFilteredCountAsync(filters);
            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return new PagedResult<ProductDTO>
            {
                Data = productDtos,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? _mapper.Map<ProductDTO>(product) : null;
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow;
            
            await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO?> UpdateProductAsync(int id, ProductDTO productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return null;

            _mapper.Map(productDto, existingProduct);
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(existingProduct);
            return _mapper.Map<ProductDTO>(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _productRepository.GetAllCategoriesAsync() ?? new List<string>();
        }
    }
} 