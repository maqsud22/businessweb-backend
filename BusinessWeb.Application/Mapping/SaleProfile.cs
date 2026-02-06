using AutoMapper;
using BusinessWeb.Application.DTOs.Sales;
using BusinessWeb.Domain.Entities;

namespace BusinessWeb.Application.Mapping;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<CreateSaleLineDto, SaleLine>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.DeletedAt, o => o.Ignore())
            .ForMember(d => d.SaleId, o => o.Ignore())
            .ForMember(d => d.Sale, o => o.Ignore())
            .ForMember(d => d.LineTotal, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore())
            .ForMember(d => d.ProductPackage, o => o.Ignore());

        CreateMap<Sale, SaleListItemDto>();
        CreateMap<SaleLine, SaleLineDetailDto>();
        CreateMap<Sale, SaleDetailDto>();
    }
}
