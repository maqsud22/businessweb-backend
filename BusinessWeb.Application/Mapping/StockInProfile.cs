using AutoMapper;
using BusinessWeb.Application.DTOs.StockIns;
using BusinessWeb.Domain.Entities;

namespace BusinessWeb.Application.Mapping;

public class StockInProfile : Profile
{
    public StockInProfile()
    {
        CreateMap<StockIn, StockInListItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.ProductPackageName, o => o.MapFrom(s => s.ProductPackage.Name));
        CreateMap<StockIn, StockInDetailDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.ProductPackageName, o => o.MapFrom(s => s.ProductPackage.Name));
        CreateMap<StockInCreateDto, StockIn>();
        CreateMap<StockInUpdateDto, StockIn>();
    }
}
