using Blog.Common.Database;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Sync;

/// <summary>
/// Elasticsearch 同步服务
/// 当配置中 EnableElasticsearchSync = false 时，所有操作均为空操作
/// </summary>
public class ElasticsearchSyncService : IElasticsearchSyncService
{
    private readonly bool _enabled;
    private readonly ILogger<ElasticsearchSyncService> _logger;

    public ElasticsearchSyncService(DatabaseSettings settings, ILogger<ElasticsearchSyncService> logger)
    {
        _enabled = settings.EnableElasticsearchSync;
        _logger = logger;

        if (_enabled)
        {
            // TODO: 初始化 ES 客户端
            // var urls = settings.Elasticsearch?.Urls?.Split(',') ?? new[] { "http://localhost:9200" };
            // _client = new ElasticClient(new ConnectionSettings(new Uri(urls[0])));
            _logger.LogInformation("Elasticsearch sync is ENABLED");
        }
        else
        {
            _logger.LogInformation("Elasticsearch sync is DISABLED");
        }
    }

    public Task SyncArticleAsync(object article)
    {
        if (!_enabled) return Task.CompletedTask;
        _logger.LogDebug("Syncing article to ES: {Article}", article);
        // TODO: 实现 ES 索引同步
        return Task.CompletedTask;
    }

    public Task DeleteArticleAsync(ulong articleId)
    {
        if (!_enabled) return Task.CompletedTask;
        _logger.LogDebug("Deleting article from ES: {Id}", articleId);
        // TODO: 实现 ES 索引删除
        return Task.CompletedTask;
    }

    public Task SyncCategoryAsync(object category)
    {
        if (!_enabled) return Task.CompletedTask;
        _logger.LogDebug("Syncing category to ES: {Category}", category);
        return Task.CompletedTask;
    }

    public Task DeleteCategoryAsync(ulong categoryId)
    {
        if (!_enabled) return Task.CompletedTask;
        _logger.LogDebug("Deleting category from ES: {Id}", categoryId);
        return Task.CompletedTask;
    }

    public Task<object?> SearchArticlesAsync(string keyword, int page, int size)
    {
        if (!_enabled)
        {
            // ES 未启用时返回 null，调用方应回退到数据库查询
            return Task.FromResult<object?>(null);
        }
        _logger.LogDebug("Searching articles in ES: {Keyword}", keyword);
        // TODO: 实现 ES 搜索
        return Task.FromResult<object?>(null);
    }
}
