using System.Linq.Expressions;
using Blog.Domain;

namespace Blog.Core.Repository;

public interface IRepository<TEntity>
    where TEntity: BaseEntity, new()
{
    TEntity Delete(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity);
    /// <summary>
    /// Explicitly save pending changes to the database.
    /// Use this when you need to batch multiple operations in a single transaction.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    TEntity? Get(ulong id);
    TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetAsync(ulong id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    List<TEntity> GetList();
    List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
    List<TEntity> GetList(int pageIndex, int pageSize);
    Task<List<TEntity>> GetListAsync();
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetListAsync(int pageIndex, int pageSize);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null);
    TEntity Insert(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity);
    TEntity Update(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}