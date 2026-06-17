namespace Blog.Core.DbContext;

/// <summary>
/// 只读 DbContext 包装器
/// 当读写分离启用时，查询操作使用此包装器路由到只读副本
/// 当读写分离禁用时，内部使用主库连接
/// </summary>
public class ReadOnlyDbContext : IDisposable
{
    public BlogDbContext Context { get; }

    public ReadOnlyDbContext(BlogDbContext context)
    {
        Context = context;
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}
