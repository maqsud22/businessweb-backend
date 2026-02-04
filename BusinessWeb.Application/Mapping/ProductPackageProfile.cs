using AutoMapper;
using BusinessWeb.Application.DTOs.ProductPackages;
using BusinessWeb.Domain.Entities;

namespace BusinessWeb.Application.Mapping;

public class ProductPackageProfile : Profile
{
    public ProductPackageProfile()
    {
        CreateMap<ProductPackage, ProductPackageListItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));
        CreateMap<ProductPackage, ProductPackageDetailDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));
        CreateMap<ProductPackageCreateDto, ProductPackage>();
        CreateMap<ProductPackageUpdateDto, ProductPackage>();
    }
}
