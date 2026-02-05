
using System.Linq.Expressions;
using Blog.Core.DbContext;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Services.LogMgrApp;

public class LogService(BlogDbContext context, ILogger<LogService> logger) : ILogService
{
    public List<LogModel> GetLogModels(int pageIndex, int pageSize)
    {
        var logs = context.LogModels.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return logs;
    }

    public LogModel? GetLogModel(Func<LogModel, bool> predicate)
    {
        var log = context.LogModels.FirstOrDefault(predicate);
        return log;
    }

    public async Task<List<LogModel>> GetLogModelsAsync(int pageIndex, int pageSize)
    {
        var logs = await context.LogModels.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return logs;
    }

    public async Task<LogModel?> GetLogModelAsync(Expression<Func<LogModel, bool>> predicate)
    {
        var log = await context.LogModels.FirstOrDefaultAsync(predicate);
        return log;
    }

}