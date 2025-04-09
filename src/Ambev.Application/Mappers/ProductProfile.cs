using AutoMapper;
using Ambev.Application.DTOs;
using Ambev.Domain.Entities;

namespace Ambev.Application.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Mapeamento de Product (Entidade) para ProductDTO (DTO)
            CreateMap<Product, ProductDTO>()
                // Mapeamento aninhado para Rating (seguindo documentação)
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingDTO 
                    {
                         // Mapeia entidade.Rating -> dto.Rating.Rate
                        Rate = src.Rating, 
                        // Count não existe na entidade, define como 0 ou outra lógica?
                        Count = 0 // Placeholder
                    }));
                // Stock é ignorado pois não está no ProductDTO

            // Mapeamento de ProductDTO (DTO) para Product (Entidade)
            CreateMap<ProductDTO, Product>()
                // Mapeia dto.Rating.Rate -> entidade.Rating
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Rate))
                // Ignorar Stock, pois não vem do DTO
                .ForMember(dest => dest.Stock, opt => opt.Ignore())
                // CreatedAt/UpdatedAt devem ser gerenciados pelo repo/serviço
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CartItems, opt => opt.Ignore()); // Ignorar coleções
        }
    }
} 