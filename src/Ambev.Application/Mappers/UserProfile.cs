using AutoMapper;
using Ambev.Application.DTOs;
using Ambev.Domain.Entities;

namespace Ambev.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new UserNameDTO { Firstname = src.Firstname, Lastname = src.Lastname }))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new UserAddressDTO
                {
                    City = src.City,
                    Street = src.Street,
                    Number = ParseNumber(src.Number), 
                    Zipcode = src.Zipcode,
                    Geolocation = new UserGeoLocationDTO { Lat = src.GeolocationLat, Long = src.GeolocationLong }
                }))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Name.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Name.Lastname))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Address.Number.ToString()))
                .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.Address.Zipcode))
                .ForMember(dest => dest.GeolocationLat, opt => opt.MapFrom(src => src.Address.Geolocation.Lat))
                .ForMember(dest => dest.GeolocationLong, opt => opt.MapFrom(src => src.Address.Geolocation.Long))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Carts, opt => opt.Ignore());
        }

        private static int ParseNumber(string number)
        {
            return int.TryParse(number, out int result) ? result : 0;
        }
    }
} 