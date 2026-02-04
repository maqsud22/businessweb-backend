using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessWeb.Application.DTOs.Stores;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Application.Interfaces.Stores;
using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Services;

public class StoreService : IStoreService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public StoreService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<List<StoreListItemDto>> GetAllAsync(CancellationToken ct = default)
        => _uow.Repo<Store>().Query()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<StoreListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

    public Task<StoreDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _uow.Repo<Store>().Query()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<StoreDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);

    public async Task<StoreDetailDto> CreateAsync(StoreCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<Store>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _uow.Repo<Store>().AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<StoreDetailDto>(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, StoreUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<Store>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        _mapper.Map(dto, entity);
        _uow.Repo<Store>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<Store>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        entity.DeletedAt = DateTime.UtcNow;
        _uow.Repo<Store>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
