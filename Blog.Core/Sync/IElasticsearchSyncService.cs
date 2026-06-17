namespace Blog.Core.Sync;

/// <summary>
/// Elasticsearch 同步服务接口
/// 当配置中 EnableElasticsearchSync = false 时，使用 NoopElasticsearchSyncService
/// </summary>
public interface IElasticsearchSyncService
{
    /// <summary>同步文章数据到 ES</summary>
    Task SyncArticleAsync(object article);
    /// <summary>从 ES 删除文章</summary>
    Task DeleteArticleAsync(ulong articleId);
    /// <summary>同步分类数据到 ES</summary>
    Task SyncCategoryAsync(object category);
    /// <summary>从 ES 删除分类</summary>
    Task DeleteCategoryAsync(ulong categoryId);
    /// <summary>搜索文章</summary>
    Task<object?> SearchArticlesAsync(string keyword, int page, int size);
}
