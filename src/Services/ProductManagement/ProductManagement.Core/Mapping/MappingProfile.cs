using AutoMapper;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Features.CreateProduct;
using ProductManagement.Core.Features.UpdateProduct;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId));

        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId));

        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CreatedBy));

        CreateMap<ProductForCreateDto, CreateProductCommand>();

        CreateMap<UpdateProductCommand, Product>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UpdatedBy));

        CreateMap<ProductForUpdateDto, UpdateProductCommand>();

        CreateMap<Category, CategoryDto>();
    }
}
