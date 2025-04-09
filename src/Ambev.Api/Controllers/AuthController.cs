using Microsoft.AspNetCore.Mvc;
using Ambev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Ambev.Domain.DTOs;

namespace Ambev.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO.Username, loginDTO.Password);
            
            if (response == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            return Ok(response);
        }
    }
} 