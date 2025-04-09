using System.ComponentModel.DataAnnotations;

namespace Ambev.Domain.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
} 