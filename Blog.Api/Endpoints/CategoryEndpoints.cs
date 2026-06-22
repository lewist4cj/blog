using Blog.Common;
using Blog.Common.Utils;
using Blog.Core.Repository;
using Blog.Domain;
using Blog.Domain.Dtos;
using SqlSugar;

namespace Blog.Api.Endpoints;

public static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoryEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetCategories);
        group.MapGet("/options", GetCategoryOptions);
        group.MapPost("/", CreateCategory).RequireAuthorization();
        group.MapDelete("/", DeleteCategories).RequireAuthorization();
        return group;
    }

    private static async Task<ApiResult> GetCategories(
        int? type,
        ulong? userId,
        string? key,
        int page = 1,
        int limit = 10,
        ISqlSugarClient? db = null)
    {
        var q = db!.Queryable<CategoryModel>()
            .LeftJoin<UserModel>((c, u) => c.UserId == u.Id);

        if (userId.HasValue)
            q = q.Where((c, u) => c.UserId == userId.Value);
        if (!string.IsNullOrEmpty(key))
            q = q.Where((c, u) => c.Title!.Contains(key));

        var total = await q.CountAsync();
        var selectQ = q.Select((c, u) => new CategoryView
        {
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            Title = c.Title,
            UserId = c.UserId,
            UserName = u.Nickname,
            ArticleCount = SqlFunc.Subqueryable<ArticleModel>().Where(a => a.CategoryId == c.Id).Count()
        });

        var list = await selectQ.OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * limit).Take(limit)
            .ToListAsync();

        return ApiResult.Success(new CategoryListResult { List = list, Count = total });
    }

    private static async Task<ApiResult> GetCategoryOptions(ISqlSugarClient db)
    {
        var list = await db.Queryable<CategoryModel>()
            .Select(c => new SelectOption { Value = c.Id, Label = c.Title })
            .ToListAsync();
        return ApiResult.Success(list);
    }

    private static async Task<ApiResult> CreateCategory(
        CreateCategoryRequest request,
        IRepository<CategoryModel> categoryRepo,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);

        var category = new CategoryModel
        {
            Title = request.Title,
            UserId = userId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        await categoryRepo.InsertAsync(category);
        await categoryRepo.SaveChangesAsync();
        return ApiResult.Success("created");
    }

    private static async Task<ApiResult> DeleteCategories(
        DeleteRequest request,
        IRepository<CategoryModel> categoryRepo)
    {
        foreach (var id in request.IdList)
        {
            var cat = await categoryRepo.GetAsync(id);
            if (cat != null)
            {
                categoryRepo.Delete(cat);
            }
        }
        await categoryRepo.SaveChangesAsync();
        return ApiResult.Success("deleted");
    }

    private static ulong? GetUserId(HttpContext ctx)
    {
        var claim = ctx.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        return ulong.TryParse(claim, out var id) ? id : null;
    }
}

public class CategoryListResult
{
    public List<CategoryView> List { get; set; } = new();
    public int Count { get; set; }
}

public class CategoryView
{
    public ulong Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Title { get; set; }
    public ulong? UserId { get; set; }
    public string? UserName { get; set; }
    public long? ArticleCount { get; set; }
}

public class CreateCategoryRequest
{
    public ulong? Id { get; set; }
    public string? Title { get; set; }
}

public class DeleteRequest
{
    public List<ulong> IdList { get; set; } = new();
}
