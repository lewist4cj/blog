
using System.Linq.Expressions;
using Blog.Common;
using Blog.Common.RespModule;
using Blog.Domain;

namespace Blog.Services.LogMgrApp; 

public interface ILogService:ITag
{
    Task<LogModel?> GetLogModelAsync(Expression<Func<LogModel, bool>> predicate);
    Task<PageResult<LogModel>> GetLogModelsAsync(int pageIndex, int pageSize);
}