using BusinessWeb.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _db;
    private readonly DbSet<T> _set;

    public EfRepository(DbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _set.FindAsync([id], ct).AsTask();

    public IQueryable<T> Query() => _set.AsQueryable();

    public Task AddAsync(T entity, CancellationToken ct = default)
        => _set.AddAsync(entity, ct).AsTask();

    public void Update(T entity) => _set.Update(entity);

    public void Remove(T entity) => _set.Remove(entity);
}
