using BusinessWeb.Application.Interfaces;
using BusinessWeb.Infrastructure.Data;
using System.Collections.Concurrent;

namespace BusinessWeb.Infrastructure.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly ConcurrentDictionary<Type, object> _repos = new();

    public EfUnitOfWork(AppDbContext db) => _db = db;

    public IRepository<T> Repo<T>() where T : class
    {
        var type = typeof(T);
        if (_repos.TryGetValue(type, out var repo)) return (IRepository<T>)repo;

        var created = new EfRepository<T>(_db);
        _repos[type] = created;
        return created;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
