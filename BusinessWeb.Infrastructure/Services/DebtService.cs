using BusinessWeb.Application.DTOs.Debts;
using BusinessWeb.Application.Exceptions;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Application.Interfaces.Debts;
using BusinessWeb.Domain.Entities;
using BusinessWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Services;

public class DebtService : IDebtService
{
    private readonly AppDbContext _db;
    private readonly IUnitOfWork _uow;

    public DebtService(AppDbContext db, IUnitOfWork uow)
    {
        _db = db;
        _uow = uow;
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
