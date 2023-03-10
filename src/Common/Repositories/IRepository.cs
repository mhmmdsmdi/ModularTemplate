using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Repositories;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    DbSet<TEntity> Entities { get; }
    IQueryable<TEntity> Table { get; }
    IQueryable<TEntity> TableNoTracking { get; }

    Task<IDbContextTransaction> CreateTransactionAsync();

    Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);

    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);

    Task SaveAsync(CancellationToken cancellationToken);
}