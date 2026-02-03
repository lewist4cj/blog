using System.Linq.Expressions;
using Blog.Core.DbContext;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Repository;

public class Repository<TEntity>(BlogDbContext context): IRepository<TEntity>
    where TEntity: BaseEntity
{
    public List<TEntity> GetList()
    {
        var dbSet = context.Set<TEntity>();
        return dbSet.ToList();
    }
    public List<TEntity> GetList(Func<TEntity,bool> predicate)
    {
        var dbSet = context.Set<TEntity>();
        return dbSet.Where(predicate).ToList();
    }
    public async Task<List<TEntity>> GetListAsync()
    {
        var dbSet = context.Set<TEntity>();
        return await dbSet.ToListAsync();
    }
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity,bool>> predicate)
    {
        var dbSet = context.Set<TEntity>();
        return  await dbSet.Where(predicate).ToListAsync();
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

    public TEntity? Get(Func<TEntity, bool> predicate)
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
        context.SaveChanges();
        return res;
    }
    
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res =  (await dbSet.AddAsync(entity)).Entity;
        await context.SaveChangesAsync();
        return res;
    }
    
    public TEntity Delete(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Remove(entity).Entity;
        context.SaveChanges();
        return res;
    }
    
    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Remove(entity).Entity;
        await context.SaveChangesAsync();
        return res;
    }
    
    public TEntity Update(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Update(entity).Entity;
        context.SaveChanges();
        return res;
    }
    
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var dbSet = context.Set<TEntity>();
        var res = dbSet.Update(entity).Entity;
        await context.SaveChangesAsync();
        return res;
    }
}