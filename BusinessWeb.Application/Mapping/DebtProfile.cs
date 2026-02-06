using AutoMapper;
using BusinessWeb.Application.DTOs.Debts;
using BusinessWeb.Domain.Entities;

namespace BusinessWeb.Application.Mapping;

public class DebtProfile : Profile
{
    public DebtProfile()
    {
        CreateMap<Debt, DebtListItemDto>();
        CreateMap<DebtPayment, DebtPaymentDto>();
        CreateMap<Debt, DebtDetailDto>();
    }
}
