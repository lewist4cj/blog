using Blog.Common;
using Blog.Common.Utils;
using Blog.Core.Repository;
using Blog.Domain;
using SqlSugar;

namespace Blog.Api.Endpoints;

public static class CollectEndpoints
{
    public static RouteGroupBuilder MapCollectEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetCollects);
        group.MapPost("/", CreateCollect).RequireAuthorization();
        group.MapDelete("/", DeleteCollects).RequireAuthorization();
        return group;
    }

    private static async Task<ApiResult> GetCollects(
        int? type,
        ulong? userId,
        string? key,
        int page = 1,
        int limit = 10,
        ISqlSugarClient? db = null,
        HttpContext? httpContext = null)
    {
        var currentUserId = GetUserId(httpContext!);
        var q = db!.Queryable<CollectModel>();

        if (type == 1 && currentUserId.HasValue)
            q = q.Where(c => c.UserId == currentUserId.Value);
        else if (type == 2 && userId.HasValue)
            q = q.Where(c => c.UserId == userId.Value);

        if (!string.IsNullOrEmpty(key))
            q = q.Where(c => c.Title!.Contains(key));

        var total = await q.CountAsync();
        var list = await q.OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * limit).Take(limit)
            .Select(c => new CollectView
            {
                Id = c.Id,
                Title = c.Title,
                Abstract = c.Abstract,
                Cover = c.Cover,
                ArticleCount = c.ArticleCount,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId
            })
            .ToListAsync();

        return ApiResult.Success(new { list, count = total });
    }

    private static async Task<ApiResult> CreateCollect(
        CreateCollectRequest request,
        IRepository<CollectModel> collectRepo,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);

        var collect = new CollectModel
        {
            Title = request.Title,
            Abstract = request.Abstract,
            UserId = userId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        await collectRepo.InsertAsync(collect);
        await collectRepo.SaveChangesAsync();
        return ApiResult.Success("created");
    }

    private static async Task<ApiResult> DeleteCollects(
        CollectDeleteRequest request,
        IRepository<CollectModel> collectRepo)
    {
        foreach (var id in request.IdList)
        {
            var collect = await collectRepo.GetAsync(id);
            if (collect != null)
            {
                collectRepo.Delete(collect);
            }
        }
        await collectRepo.SaveChangesAsync();
        return ApiResult.Success("deleted");
    }

    private static ulong? GetUserId(HttpContext ctx)
    {
        var claim = ctx.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        return ulong.TryParse(claim, out var id) ? id : null;
    }
}

public class CollectView
{
    public ulong Id { get; set; }
    public string? Title { get; set; }
    public string? Abstract { get; set; }
    public string? Cover { get; set; }
    public long? ArticleCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public ulong? UserId { get; set; }
}

public class CreateCollectRequest
{
    public ulong? Id { get; set; }
    public string? Title { get; set; }
    public string? Abstract { get; set; }
}

public class CollectDeleteRequest
{
    public List<ulong> IdList { get; set; } = new();
}
