using Ardalis.Specification.EntityFrameworkCore;
using ChefKnife.MenuReader.Data.Entities;

namespace ChefKnife.MenuReader.Data.Repositories;

public class Repository<T>(MenuReaderDbContext dbContext) :
    RepositoryBase<T>(dbContext), IRepository<T> where T : class, IEntity
{
    public virtual async Task<T> AddAsync(T entity, bool saveChanges, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Add(entity);

        if (saveChanges) await SaveChangesAsync(cancellationToken);

        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, bool saveChanges,
        CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().AddRange(entities);

        if (saveChanges) await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges,
        CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().RemoveRange(entities);

        if (saveChanges) await SaveChangesAsync(cancellationToken);
    }

    public virtual Task ReloadAsync(T entity, CancellationToken cancellationToken = default)
    {
        return dbContext.Entry(entity).ReloadAsync(cancellationToken);
    }
}
