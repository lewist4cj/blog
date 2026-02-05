using System.Linq.Expressions;
using Blog.Domain;

namespace Blog.Core.Repository;

public interface IRepository<TEntity>
    where TEntity: BaseEntity
{
    TEntity Delete(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity);
    TEntity? Get(ulong id);
    TEntity? Get(Func<TEntity, bool> predicate);
    Task<TEntity?> GetAsync(ulong id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    List<TEntity> GetList();
    List<TEntity> GetList(Func<TEntity, bool> predicate);
    List<TEntity> GetList(int pageIndex, int pageSize);
    Task<List<TEntity>> GetListAsync();
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetListAsync(int pageIndex, int pageSize);
    TEntity Insert(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity);
    TEntity Update(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}