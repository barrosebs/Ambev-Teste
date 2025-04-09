using System.ComponentModel.DataAnnotations;

namespace Ambev.Application.DTOs
{
    public class RatingDTO
    {
        [Required(ErrorMessage = "A taxa é obrigatória")]
        [Range(0, 5, ErrorMessage = "A taxa deve estar entre 0 e 5")]
        public double Rate { get; set; }

        [Required(ErrorMessage = "A contagem é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "A contagem deve ser maior ou igual a zero")]
        public int Count { get; set; }
    }
} 