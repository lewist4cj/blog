
namespace Blog.Common.RespModule;

public class PageResult<T>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; } = 0;
    public List<T>? List { get; set; }

    public PageResult(int pageIndex, int pageSize, int totalCount, List<T>? list)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        List = list;
    }
}