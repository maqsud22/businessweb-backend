using AutoMapper;
using BusinessWeb.Application.DTOs.Products;
using BusinessWeb.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessWeb.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductListItemDto>();
        CreateMap<Product, ProductDetailDto>();

        CreateMap<ProductCreateDto, Product>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Packages, o => o.Ignore());

        CreateMap<ProductUpdateDto, Product>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.Packages, o => o.Ignore());
    }
}
