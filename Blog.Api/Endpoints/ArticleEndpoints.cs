using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.Dtos;
using Blog.Services.ArticleApp;

namespace Blog.Api.Endpoints;

public static class ArticleEndpoints
{
    public static RouteGroupBuilder MapArticleEndpoints(this RouteGroupBuilder group)
    {
        // Article CRUD
        group.MapGet("/", GetArticles);
        group.MapGet("/{id:long}", GetArticle);
        group.MapPost("/", CreateArticle).RequireAuthorization();
        group.MapPut("/", UpdateArticle).RequireAuthorization();
        group.MapDelete("/{id:long}", DeleteArticle).RequireAuthorization();
        group.MapPost("/examine", ExamineArticle).RequireAuthorization();

        // Digg & Collect
        group.MapGet("/digg/{id:long}", ToggleDigg).RequireAuthorization();
        group.MapPost("/collect", CollectArticle).RequireAuthorization();
        group.MapDelete("/collect", RemoveArticleCollect).RequireAuthorization();

        // History
        group.MapGet("/history", GetHistory).RequireAuthorization();
        group.MapPost("/history", RecordHistory).RequireAuthorization();
        group.MapDelete("/history", DeleteHistory).RequireAuthorization();

        // Article top
        group.MapPost("/{id:long}/top", ToggleTopArticle).RequireAuthorization();

        // Options
        group.MapGet("/tag/options", GetTagOptions);
        group.MapGet("/auth_recommend", GetAuthRecommend);
        group.MapGet("/article_recommend", GetArticleRecommend);

        return group;
    }

    private static async Task<ApiResult> GetArticles(
        [AsParameters] ArticleQuery query,
        IArticleService articleService)
    {
        var result = await articleService.GetArticlesAsync(query);
        return ApiResult.Success(result);
    }

    private static async Task<ApiResult> GetArticle(
        long id,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        var result = await articleService.GetArticleAsync((ulong)id, userId);
        if (result == null) return ApiResult.Failure(Code.NotFound);
        return ApiResult.Success(result);
    }

    private static async Task<ApiResult> CreateArticle(
        ArticleAddRequest request,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        var id = await articleService.CreateArticleAsync(request, userId.Value);
        return ApiResult.Success(new { id });
    }

    private static async Task<ApiResult> UpdateArticle(
        ArticleEditRequest request,
        IArticleService articleService)
    {
        await articleService.UpdateArticleAsync(request);
        return ApiResult.Success("updated");
    }

    private static async Task<ApiResult> DeleteArticle(
        long id,
        IArticleService articleService)
    {
        await articleService.DeleteArticleAsync((ulong)id);
        return ApiResult.Success("deleted");
    }

    private static async Task<ApiResult> ExamineArticle(
        ArticleExamineRequest request,
        IArticleService articleService)
    {
        await articleService.ExamineArticleAsync(request);
        return ApiResult.Success("examined");
    }

    private static async Task<ApiResult> ToggleDigg(
        long id,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        var isDigg = await articleService.ToggleDiggAsync((ulong)id, userId.Value);
        return ApiResult.Success(new { isDigg });
    }

    private static async Task<ApiResult> CollectArticle(
        ArticleCollectRequest request,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        await articleService.CollectArticleAsync(request.ArticleId, request.CollectId, userId.Value);
        return ApiResult.Success("ok");
    }

    private static async Task<ApiResult> RemoveArticleCollect(
        RemoveCollectRequest request,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        await articleService.RemoveArticleCollectAsync(request.CollectId, request.ArticleIds, userId.Value);
        return ApiResult.Success("ok");
    }

    private static async Task<ApiResult> GetHistory(
        int type,
        string? key,
        int page,
        int limit,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        var result = await articleService.GetHistoryAsync(type, key, page, limit, userId);
        return ApiResult.Success(result);
    }

    private static async Task<ApiResult> RecordHistory(
        HistoryRecordRequest request,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        await articleService.RecordHistoryAsync(request.ArticleId, userId.Value);
        return ApiResult.Success("ok");
    }

    private static async Task<ApiResult> DeleteHistory(
        DeleteHistoryRequest request,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        await articleService.DeleteHistoryAsync(request.IdList, userId.Value);
        return ApiResult.Success("ok");
    }

    private static async Task<ApiResult> ToggleTopArticle(
        long id,
        TopArticleRequest request,
        IArticleService articleService,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);
        await articleService.ToggleTopAsync((ulong)id, request.Type, userId.Value);
        return ApiResult.Success("ok");
    }

    private static async Task<ApiResult> GetTagOptions(IArticleService articleService)
    {
        var tags = await articleService.GetTagOptionsAsync();
        return ApiResult.Success(tags);
    }

    private static async Task<ApiResult> GetAuthRecommend(IArticleService articleService)
    {
        // Returns recommended authors
        return ApiResult.Success(new List<object>());
    }

    private static async Task<ApiResult> GetArticleRecommend(IArticleService articleService)
    {
        var list = await articleService.GetRecommendAsync(null);
        return ApiResult.Success(new { list });
    }

    private static ulong? GetUserId(HttpContext ctx)
    {
        var claim = ctx.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        return ulong.TryParse(claim, out var id) ? id : null;
    }
}

// Additional DTOs for endpoints
public class RemoveCollectRequest
{
    public ulong CollectId { get; set; }
    public List<ulong> ArticleIds { get; set; } = new();
}

public class HistoryRecordRequest
{
    public ulong ArticleId { get; set; }
}

public class DeleteHistoryRequest
{
    public List<ulong> IdList { get; set; } = new();
}

public class TopArticleRequest
{
    public int Type { get; set; }
}
