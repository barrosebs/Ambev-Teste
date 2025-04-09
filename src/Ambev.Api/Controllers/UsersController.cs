using Microsoft.AspNetCore.Mvc;
using Ambev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Ambev.Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ambev.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserDTO> _validator;

        public UsersController(IUserService userService, IValidator<UserDTO> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<UserDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<UserDTO>>> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery(Name = "_order")] string? order = null,
            [FromQuery] Dictionary<string, string>? filters = null)
        {
            var pagedResult = await _userService.GetUsersAsync(page, pageSize, order, filters);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ApiErrorResponse { Type = "ResourceNotFound", Error = "Usuário não encontrado", Detail = $"Usuário com ID {id} não encontrado." });
            }
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                userDto.Id = 0;
                var createdUser = await _userService.CreateUserAsync(userDto);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id, version = "1.0" }, createdUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiErrorResponse { Type = "ValidationError", Error = "Erro de validação", Detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userDto.Id = id;

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, userDto);
                if (updatedUser == null)
                {
                    return NotFound(new ApiErrorResponse { Type = "ResourceNotFound", Error = "Usuário não encontrado", Detail = $"Usuário com ID {id} não encontrado." });
                }
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiErrorResponse { Type = "ValidationError", Error = "Erro de validação", Detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                    return NotFound(new { message = "Usuário não encontrado" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao excluir usuário", error = ex.Message });
            }
        }
    }
} 