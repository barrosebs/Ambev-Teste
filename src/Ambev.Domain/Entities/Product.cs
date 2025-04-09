using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ambev.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
} 