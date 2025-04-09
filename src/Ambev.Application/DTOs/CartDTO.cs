using System;
using System.ComponentModel.DataAnnotations;

namespace Ambev.Application.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public required int UserId { get; set; }

        [Required(ErrorMessage = "A lista de itens é obrigatória")]
        public required List<CartItemDTO> Items { get; set; }

        public decimal Total { get; set; }
    }

    public class CartItemDTO
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório")]
        public required int ProductId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public required int Quantity { get; set; }

        public decimal Price { get; set; }
    }
} 