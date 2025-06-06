using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ambev.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public string GeolocationLat { get; set; } = string.Empty;
        public string GeolocationLong { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended
    }

    public enum UserRole
    {
        Customer,
        Manager,
        Admin
    }
} 