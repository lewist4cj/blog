
using System.Linq.Expressions;
using Blog.Common;
using Blog.Common.RespModule;
using Blog.Domain;

namespace Blog.Services.LogMgrApp; 

public interface ILogService:ITag
{
    LogModel? GetLogModel(Func<LogModel, bool> predicate);
    Task<LogModel?> GetLogModelAsync(Expression<Func<LogModel, bool>> predicate);
    PageResult<LogModel> GetLogModels(int pageIndex, int pageSize);
    Task<PageResult<LogModel>> GetLogModelsAsync(int pageIndex, int pageSize);
}