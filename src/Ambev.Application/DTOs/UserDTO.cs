using System.ComponentModel.DataAnnotations;

namespace Ambev.Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        public string? Password { get; set; }

        [Required]
        public UserNameDTO Name { get; set; } = new UserNameDTO();

        [Required]
        public UserAddressDTO Address { get; set; } = new UserAddressDTO();

        public string Phone { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
} 