
using System.Linq.Expressions;
using Blog.Common.RespModule;
using Blog.Core.Repository;
using Blog.Domain;

namespace Blog.Services.LogMgrApp;

public class LogService(IRepository<LogModel> repository) : ILogService
{
    public async Task<PageResult<LogModel>> GetLogModelsAsync(int pageIndex, int pageSize)
    {
        var logs = await repository.GetListAsync(pageIndex, pageSize);
        var totalCount = await repository.GetCountAsync();
        return new PageResult<LogModel>(pageIndex, pageSize, totalCount, logs);
    }

    public async Task<LogModel?> GetLogModelAsync(Expression<Func<LogModel, bool>> predicate)
    {
        return await repository.GetAsync(predicate);
    }
}