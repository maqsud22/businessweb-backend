namespace BusinessWeb.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<T> Repo<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
