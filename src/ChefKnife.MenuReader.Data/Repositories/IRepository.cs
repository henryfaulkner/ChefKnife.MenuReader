using Ardalis.Specification;
using ChefKnife.MenuReader.Data.Entities;

namespace ChefKnife.MenuReader.Data.Repositories;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IEntity
{
    Task<T> AddAsync(T entity, bool saveChanges, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, bool saveChanges, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges, CancellationToken cancellationToken = default);
    Task ReloadAsync(T entity, CancellationToken cancellationToken = default);
}
