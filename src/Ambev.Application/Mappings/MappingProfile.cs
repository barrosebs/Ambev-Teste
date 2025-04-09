using Ambev.Application.DTOs;
using Ambev.Domain.Entities;
using AutoMapper;

namespace Ambev.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingDTO
                {
                    Rate = src.Rating,
                    Count = src.Stock
                }));

            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Rate))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Rating.Count));
        }
    }
} 