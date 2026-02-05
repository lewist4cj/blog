
using System.Linq.Expressions;
using Blog.Common.RespModule;
using Blog.Core.Repository;
using Blog.Domain;

namespace Blog.Services.LogMgrApp;

public class LogService(IRepository<LogModel> repository) : ILogService
{
    public PageResult<LogModel> GetLogModels(int pageIndex, int pageSize)
    {
        var logs = repository.GetList(pageIndex,pageSize);
        var res  = new  PageResult<LogModel>(pageIndex,pageSize,logs.Count,logs);
        return res;
    }

    public LogModel? GetLogModel(Func<LogModel, bool> predicate)
    {
        var log = repository.Get(predicate);
        return log;
    }

    public async Task<PageResult<LogModel>> GetLogModelsAsync(int pageIndex, int pageSize)
    {
        var logs = await repository.GetListAsync(pageIndex,pageSize);
        var res  = new  PageResult<LogModel>(pageIndex,pageSize,logs.Count,logs);
        return res;
    }

    public async Task<LogModel?> GetLogModelAsync(Expression<Func<LogModel, bool>> predicate)
    {
        var log = await repository.GetAsync(predicate);
        return log;
    }

}