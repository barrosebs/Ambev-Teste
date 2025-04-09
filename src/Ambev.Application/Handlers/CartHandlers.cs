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
    public class CartHandlers :
        IRequestHandler<CreateCartCommand, CartDTO>,
        IRequestHandler<UpdateCartCommand, CartDTO?>,
        IRequestHandler<DeleteCartCommand, bool>,
        IRequestHandler<GetCartByIdQuery, CartDTO?>,
        IRequestHandler<GetAllCartsQuery, PagedResult<CartDTO>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartHandlers(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CartDTO> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            if (request?.Cart == null)
                throw new ArgumentNullException(nameof(request.Cart));

            var cart = _mapper.Map<Cart>(request.Cart);
            cart.CreatedAt = DateTime.UtcNow;
            
            await _cartRepository.AddAsync(cart);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO?> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            if (request?.Cart == null)
                throw new ArgumentNullException(nameof(request.Cart));

            var cart = await _cartRepository.GetByIdAsync(request.Id);
            if (cart == null)
                return null;

            _mapper.Map(request.Cart, cart);
            cart.UpdatedAt = DateTime.UtcNow;
            
            await _cartRepository.UpdateAsync(cart);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.Id);
            if (cart == null)
                return false;

            await _cartRepository.DeleteAsync(cart);
            return true;
        }

        public async Task<CartDTO?> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.Id);
            return cart != null ? _mapper.Map<CartDTO>(cart) : null;
        }

        public async Task<PagedResult<CartDTO>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
        {
            var carts = await _cartRepository.GetPagedFilteredAndSortedAsync(request.Page, request.Size, request.Order, request.Filters);
            var totalItems = await _cartRepository.GetFilteredCountAsync(request.Filters);
            var cartDtos = _mapper.Map<IEnumerable<CartDTO>>(carts);

            return new PagedResult<CartDTO>
            {
                Data = cartDtos,
                CurrentPage = request.Page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.Size)
            };
        }
    }
} 