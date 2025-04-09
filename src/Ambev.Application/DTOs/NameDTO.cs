using System.ComponentModel.DataAnnotations;

namespace Ambev.Application.DTOs
{
    public class NameDTO
    {
        [Required(ErrorMessage = "O primeiro nome é obrigatório")]
        [StringLength(50, ErrorMessage = "O primeiro nome não pode ter mais de 50 caracteres")]
        public required string Firstname { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(50, ErrorMessage = "O sobrenome não pode ter mais de 50 caracteres")]
        public required string Lastname { get; set; }
    }
} 