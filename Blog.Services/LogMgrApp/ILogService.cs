
using System.Linq.Expressions;
using Blog.Common;
using Blog.Domain;

namespace Blog.Services.LogMgrApp; 

public interface ILogService:ITag
{
    LogModel? GetLogModel(Func<LogModel, bool> predicate);
    Task<LogModel?> GetLogModelAsync(Expression<Func<LogModel, bool>> predicate);
    List<LogModel> GetLogModels(int pageIndex, int pageSize);
    Task<List<LogModel>> GetLogModelsAsync(int pageIndex, int pageSize);
}