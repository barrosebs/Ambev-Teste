using Ambev.Application.DTOs;
using Ambev.Application.Interfaces;
using Ambev.Application.Mappings;
using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResult<CartDTO>> GetAllAsync(int page = 1, int pageSize = 10, string? order = null, IDictionary<string, string>? filters = null)
        {
            var carts = await _cartRepository.GetPagedFilteredAndSortedAsync(page, pageSize, order, filters);
            var totalItems = await _cartRepository.GetFilteredCountAsync(filters);
            var cartDtos = _mapper.Map<IEnumerable<CartDTO>>(carts);

            return new PagedResult<CartDTO>
            {
                Data = cartDtos,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<CartDTO?> GetByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            return cart != null ? _mapper.Map<CartDTO>(cart) : null;
        }

        public async Task<CartDTO> CreateAsync(CartDTO cartDTO)
        {
            if (cartDTO == null)
                throw new ArgumentNullException(nameof(cartDTO));

            var cart = _mapper.Map<Cart>(cartDTO);
            cart.CreatedAt = DateTime.UtcNow;
            
            await _cartRepository.AddAsync(cart);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO?> UpdateAsync(int id, CartDTO cartDTO)
        {
            if (cartDTO == null)
                throw new ArgumentNullException(nameof(cartDTO));

            var existingCart = await _cartRepository.GetByIdAsync(id);
            if (existingCart == null)
                return null;

            _mapper.Map(cartDTO, existingCart);
            existingCart.UpdatedAt = DateTime.UtcNow;
            
            await _cartRepository.UpdateAsync(existingCart);
            return _mapper.Map<CartDTO>(existingCart);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
                return false;

            await _cartRepository.DeleteAsync(cart);
            return true;
        }
    }
} 