using System.ComponentModel.DataAnnotations;

namespace Ambev.Application.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        // Seguindo a documentação products-api.md
        public ProductRatingDTO Rating { get; set; } = new ProductRatingDTO();

        // Stock não está na documentação, então omitido do DTO.
    }
} 