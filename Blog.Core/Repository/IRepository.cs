using System.Linq.Expressions;
using Blog.Domain;

namespace Blog.Core.Repository;

public interface IRepository<TEntity>
    where TEntity: BaseEntity
{
    List<TEntity> GetList();
    List<TEntity> GetList(Func<TEntity,bool> predicate);
    Task<List<TEntity>> GetListAsync();
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity,bool>> predicate);
    TEntity? Get(ulong id);
    Task<TEntity?> GetAsync(ulong id);
    TEntity Insert(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity);
    TEntity Delete(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity);
    TEntity Update(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}