using AutoMapper;
using BusinessWeb.Application.DTOs.Stores;
using BusinessWeb.Domain.Entities;

namespace BusinessWeb.Application.Mapping;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreListItemDto>();
        CreateMap<Store, StoreDetailDto>();
        CreateMap<StoreCreateDto, Store>();
        CreateMap<StoreUpdateDto, Store>();
    }
}
