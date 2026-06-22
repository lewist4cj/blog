using System.Linq.Expressions;
using Blog.Domain;
using SqlSugar;

namespace Blog.Core.Repository;

public class Repository<TEntity>(ISqlSugarClient db) : IRepository<TEntity> where TEntity : BaseEntity, new()
{
    public List<TEntity> GetList()
        => db.Queryable<TEntity>().ToList();

    public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        => db.Queryable<TEntity>().Where(predicate).ToList();

    public async Task<List<TEntity>> GetListAsync()
        => await db.Queryable<TEntity>().ToListAsync();

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        => await db.Queryable<TEntity>().Where(predicate).ToListAsync();

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate != null)
            return await db.Queryable<TEntity>().Where(predicate).CountAsync();
        return await db.Queryable<TEntity>().CountAsync();
    }

    public List<TEntity> GetList(int pageIndex, int pageSize)
        => db.Queryable<TEntity>().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

    public async Task<List<TEntity>> GetListAsync(int pageIndex, int pageSize)
        => await db.Queryable<TEntity>().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

    public TEntity? Get(ulong id)
        => db.Queryable<TEntity>().First(it => it.Id == id);

    public async Task<TEntity?> GetAsync(ulong id)
        => await db.Queryable<TEntity>().FirstAsync(it => it.Id == id);

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        => db.Queryable<TEntity>().First(predicate);

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        => await db.Queryable<TEntity>().FirstAsync(predicate);

    public TEntity Insert(TEntity entity)
        => db.Insertable(entity).ExecuteReturnEntity();

    public async Task<TEntity> InsertAsync(TEntity entity)
        => await db.Insertable(entity).ExecuteReturnEntityAsync();

    public TEntity Delete(TEntity entity)
    {
        db.Deleteable(entity).ExecuteCommand();
        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        await db.Deleteable(entity).ExecuteCommandAsync();
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        db.Updateable(entity).ExecuteCommand();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        await db.Updateable(entity).ExecuteCommandAsync();
        return entity;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // SqlSugar 每次操作自动提交，无需手动 SaveChanges
        return await Task.FromResult(0);
    }
}
