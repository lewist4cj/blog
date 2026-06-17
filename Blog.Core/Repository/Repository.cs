using System.Linq.Expressions;
using Blog.Core.DbContext;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Repository;

public class Repository<TEntity>(BlogDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
    public List<TEntity> GetList()
    {
        var dbSet = context.Set<TEntity>();
        return dbSet.ToList();
    }
    public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
    {
        var dbSet = context.Set<TEntity>();
        return dbSet.Where(predicate).ToList();
    }
    public async Task<List<TEntity>> GetListAsync()
    {
        var dbSet = context.Set<TEntity>();
        return await dbSet.ToListAsync();
    }
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var dbSet = context.Set<TEntity>();
        return await dbSet.Where(predicate).ToListAsync();
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var dbSet = context.Set<TEntity>();
        if (predicate != null)
            return await dbSet.CountAsync(predicate);
        return await dbSet.CountAsync();
    }

    public List<TEntity> GetList(int pageIndex, int pageSize)
    {
        var dbSet = context.Set<TEntity>();
        return [.. dbSet.Skip((pageIndex - 1) * pageSize).Take(pageSize)];
    }

    public async Task<List<TEntity>> GetListAsync(int pageIndex, int pageSize)
    {
        var dbSet = context.Set<TEntity>();
        return await dbSet.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public TEntity? Get(ulong id)
    {
        var dbSet = context.Set<TEntity>();
        return dbSet.FirstOrDefault(c => c.Id == id);
    }

    public async Task<TEntity?> GetAsync(ulong id)
    {
        var dbSet = context.Set<TEntity>();
        return await dbSet.FirstOrDefaultAsync(c => c.Id == id);
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        var dbSet = context.Set<TEntity>();
        return dbSet.FirstOrDefault(predicate);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var dbSet = context.Set<TEntity>();
        return await dbSet.FirstOrDefaultAsync(predicate);
    }

    public TEntity Insert(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Add(entity).Entity;
        return res;
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = (await dbSet.AddAsync(entity)).Entity;
        return res;
    }

    public TEntity Delete(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Remove(entity).Entity;
        return res;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Remove(entity).Entity;
        return res;
    }

    public TEntity Update(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Update(entity).Entity;
        return res;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Update(entity).Entity;
        return res;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}