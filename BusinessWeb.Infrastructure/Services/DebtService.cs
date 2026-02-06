 codex/finalize-production-ready-backend-for-businessweb-3s06y9
﻿using BusinessWeb.Application.DTOs.Debts;

using BusinessWeb.Application.DTOs.Debts;
 main
using BusinessWeb.Application.Exceptions;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Application.Interfaces.Debts;
using BusinessWeb.Domain.Entities;
using BusinessWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
 codex/finalize-production-ready-backend-for-businessweb-3s06y9

namespace BusinessWeb.Infrastructure.Services;

public class DebtService : IDebtService
{
    private readonly AppDbContext _db;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ILogger<DebtService> _logger;

    public DebtService(AppDbContext db, IUnitOfWork uow, IMapper mapper, ILogger<DebtService> logger)
    {
        _db = db;
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public Task<List<DebtListItemDto>> GetAllAsync(CancellationToken ct = default)
        => _db.Debts.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<DebtListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

    public async Task<DebtDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var debt = await _db.Debts.AsNoTracking()
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return debt is null ? null : _mapper.Map<DebtDetailDto>(debt);
    }

    public async Task<DebtPayResultDto> PayAsync(DebtPayRequestDto dto, CancellationToken ct = default)
    {
        var debt = await _uow.Repo<Debt>().Query()
            .FirstOrDefaultAsync(x => x.Id == dto.DebtId, ct);

        if (debt is null)
            throw new AppException("Debt topilmadi.", 404, "not_found");

        if (debt.IsClosed)
            throw new AppException("Debt allaqachon yopilgan.", 409, "conflict");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        debt.Paid += dto.Amount;
        debt.IsClosed = debt.Paid >= debt.Total;

        var payment = new DebtPayment
        {
            Id = Guid.NewGuid(),
            DebtId = debt.Id,
            Amount = dto.Amount,
            Date = dto.Date ?? DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Repo<DebtPayment>().AddAsync(payment, ct);
        await _uow.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        _logger.LogInformation("Debt payment recorded. DebtId={DebtId}, Amount={Amount}, IsClosed={IsClosed}", debt.Id, dto.Amount, debt.IsClosed);

        return new DebtPayResultDto
        {
            DebtId = debt.Id,
            PaymentId = payment.Id,
            Total = debt.Total,
            Paid = debt.Paid,
            Remaining = Math.Max(0, debt.Total - debt.Paid),
            IsClosed = debt.IsClosed
        };
    }
}

using Microsoft.Extensions.Logging;
 main
