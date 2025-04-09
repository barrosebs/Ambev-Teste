using Ambev.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ambev.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductDTO>>> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery(Name = "_order")] string? order = null,
            [FromQuery] Dictionary<string, string>? filters = null)
        {
            var pagedResult = await _productService.GetProductsAsync(page, pageSize, order, filters);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ApiErrorResponse { Type = "ResourceNotFound", Error = "Produto não encontrado", Detail = $"Produto com ID {id} não encontrado." });
            }
            return Ok(product);
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("category/{category}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductDTO>>> GetProductsByCategory(
            string category,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery(Name = "_order")] string? order = null)
        {
            var filters = new Dictionary<string, string> { { "Category", category } };
            var pagedResult = await _productService.GetProductsAsync(page, pageSize, order, filters);
            return Ok(pagedResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                productDto.Id = 0;
                var createdProduct = await _productService.CreateProductAsync(productDto);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id, version = "1.0" }, createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse { Type = "ServerError", Error = "Erro ao criar produto", Detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            productDto.Id = id;
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
                if (updatedProduct == null)
                {
                    return NotFound(new ApiErrorResponse { Type = "ResourceNotFound", Error = "Produto não encontrado", Detail = $"Produto com ID {id} não encontrado." });
                }
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse { Type = "ServerError", Error = "Erro ao atualizar produto", Detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound(new ApiErrorResponse { Type = "ResourceNotFound", Error = "Produto não encontrado", Detail = $"Produto com ID {id} não encontrado." });
            }
            return NoContent();
        }
    }
} 