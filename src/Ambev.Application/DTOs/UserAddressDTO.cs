namespace Ambev.Application.DTOs
{
    public class UserAddressDTO
    {
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public int Number { get; set; } // Number can be int according to users-api.md
        public string Zipcode { get; set; } = string.Empty;
        public UserGeoLocationDTO Geolocation { get; set; } = new UserGeoLocationDTO();
    }
} 