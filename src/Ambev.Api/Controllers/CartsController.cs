using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Ambev.Application.DTOs;
using Ambev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDTO>>> GetAll()
        {
            var carts = await _cartService.GetAllAsync();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartDTO>> GetById(int id)
        {
            var cart = await _cartService.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<CartDTO>> Create([FromBody] CartDTO cartDTO)
        {
            var createdCart = await _cartService.CreateAsync(cartDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdCart.Id }, createdCart);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CartDTO>> Update(int id, [FromBody] CartDTO cartDTO)
        {
            var updatedCart = await _cartService.UpdateAsync(id, cartDTO);
            if (updatedCart == null)
                return NotFound();

            return Ok(updatedCart);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _cartService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 