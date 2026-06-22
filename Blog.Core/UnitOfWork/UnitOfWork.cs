using SqlSugar;

namespace Blog.Core.UnitOfWork;

public class UnitOfWork(ISqlSugarClient db) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // SqlSugar 每次操作自动提交，无需手动 SaveChanges
        return await Task.FromResult(0);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        db.Ado.BeginTran();
        await Task.CompletedTask;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        db.Ado.CommitTran();
        await Task.CompletedTask;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        db.Ado.RollbackTran();
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        db.Ado.RollbackTran();
    }
}
