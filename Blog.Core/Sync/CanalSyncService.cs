using Blog.Common.Database;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Sync;

/// <summary>
/// Canal CDC 同步服务（可选）
/// 监听 MySQL/PostgreSQL 的 binlog 变更并同步到 ES 或清理缓存
/// 当配置中 EnableCanalSync = false 时，此服务不执行任何操作
/// </summary>
public class CanalSyncService : BackgroundService
{
    private readonly bool _enabled;
    private readonly CanalSettings? _settings;
    private readonly ILogger<CanalSyncService> _logger;

    public CanalSyncService(DatabaseSettings dbSettings, ILogger<CanalSyncService> logger)
    {
        _enabled = dbSettings.EnableCanalSync;
        _settings = dbSettings.Canal;
        _logger = logger;

        if (_enabled)
        {
            _logger.LogInformation("Canal CDC sync is ENABLED (host: {Host}:{Port}, instance: {Instance})",
                _settings?.Host, _settings?.Port, _settings?.Instance);
        }
        else
        {
            _logger.LogInformation("Canal CDC sync is DISABLED");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_enabled)
        {
            _logger.LogDebug("Canal sync disabled, skipping background service");
            return;
        }

        _logger.LogInformation("Canal sync service starting...");

        try
        {
            // TODO: 初始化 Canal 客户端连接
            // 使用 canal-java-client 或自定义 TCP 客户端连接 Canal Server
            // canalConnector = CanalConnectors.NewSingleConnector(
            //     new InetSocketAddress(_settings!.Host, _settings.Port),
            //     _settings.Username ?? "",
            //     _settings.Password ?? "",
            //     _settings.Instance);

            while (!stoppingToken.IsCancellationRequested)
            {
                // TODO: 连接 Canal Server，获取 binlog 变更事件
                // 根据变更类型同步到 ES 或清理缓存
                await Task.Delay(5000, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // 正常关闭
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Canal sync service error");
        }
        finally
        {
            // TODO: 清理 Canal 连接
            _logger.LogInformation("Canal sync service stopped");
        }
    }
}
