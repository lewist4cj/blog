using Blog.Common;
using Blog.Common.Utils;
using Blog.Core.Repository;
using Blog.Domain;
using SqlSugar;

namespace Blog.Api.Endpoints;

public static class CommentEndpoints
{
    public static RouteGroupBuilder MapCommentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetComments);
        group.MapPost("/", CreateComment).RequireAuthorization();
        group.MapDelete("/{id:long}", DeleteComment).RequireAuthorization();
        group.MapGet("/tree/{articleId:long}", GetCommentTree);
        group.MapGet("/digg/{id:long}", DiggComment).RequireAuthorization();
        return group;
    }

    private static async Task<ApiResult> GetComments(
        int? type,
        string? key,
        int page = 1,
        int limit = 10,
        ISqlSugarClient? db = null,
        HttpContext? httpContext = null)
    {
        var userId = GetUserId(httpContext!);
        // Filter before Select to avoid anonymous type issues
        var baseQ = db!.Queryable<CommentModel>()
            .LeftJoin<UserModel>((c, u) => c.UserId == u.Id);

        if (type == 1)
            baseQ = baseQ.Where((c, u) => c.UserId == userId);
        else if (type == 2)
            baseQ = baseQ.Where((c, u) => SqlFunc.Subqueryable<ArticleModel>()
                .Where(a => a.Id == c.ArticleId && a.UserId == userId).Any());

        if (!string.IsNullOrEmpty(key))
            baseQ = baseQ.Where((c, u) => c.Content!.Contains(key));

        var total = await baseQ.CountAsync();
        var query = baseQ.Select((c, u) => new CommentView
        {
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            Content = c.Content,
            ArticleId = c.ArticleId,
            UserId = c.UserId,
            ParentId = c.ParentId,
            RootParentId = c.RootParentId,
            DiggCount = c.DiggCount,
            UserNickname = u.Nickname,
            UserAvatar = u.Avatar
        });

        var list = await query.OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * limit).Take(limit)
            .ToListAsync();

        return ApiResult.Success(new { list, count = total });
    }

    private static async Task<ApiResult> CreateComment(
        CreateCommentRequest request,
        IRepository<CommentModel> commentRepo,
        HttpContext httpContext)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return ApiResult.Failure(Code.Unauthorized);

        var comment = new CommentModel
        {
            Content = request.Content,
            ArticleId = request.ArticleId,
            UserId = userId,
            ParentId = request.ParentId ?? 0,
            RootParentId = request.RootParentId ?? 0,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        await commentRepo.InsertAsync(comment);

        // increment comment count on article
        var db = httpContext.RequestServices.GetRequiredService<ISqlSugarClient>();
        await db.Updateable<ArticleModel>()
            .SetColumns(a => a.CommentCount == (a.CommentCount ?? 0) + 1)
            .Where(a => a.Id == request.ArticleId)
            .ExecuteCommandAsync();

        await commentRepo.SaveChangesAsync();
        return ApiResult.Success("created");
    }

    private static async Task<ApiResult> DeleteComment(
        long id,
        IRepository<CommentModel> commentRepo,
        HttpContext httpContext)
    {
        var comment = await commentRepo.GetAsync((ulong)id);
        if (comment == null) return ApiResult.Failure(Code.NotFound);
        commentRepo.Delete(comment);
        await commentRepo.SaveChangesAsync();
        return ApiResult.Success("deleted");
    }

    private static async Task<ApiResult> GetCommentTree(
        long articleId,
        int page = 1,
        int limit = 10,
        ISqlSugarClient? db = null)
    {
        var q = db!.Queryable<CommentModel>()
            .Where(c => c.ArticleId == (ulong)articleId && c.ParentId == 0)
            .LeftJoin<UserModel>((c, u) => c.UserId == u.Id)
            .Select((c, u) => new CommentTreeItem
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                Nickname = u.Nickname,
                Avatar = u.Avatar,
                DiggCount = c.DiggCount,
                ReplyCount = SqlFunc.Subqueryable<CommentModel>()
                    .Where(r => r.RootParentId == c.Id).Count()
            });

        var total = await q.CountAsync();
        var list = await q.OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * limit).Take(limit)
            .ToListAsync();

        foreach (var item in list)
        {
            item.Replies = await db!.Queryable<CommentModel>()
                .Where(c => c.RootParentId == item.Id)
                .LeftJoin<UserModel>((c, u) => c.UserId == u.Id)
                .Select((c, u) => new CommentReplyItem
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId,
                    Nickname = u.Nickname,
                    Avatar = u.Avatar,
                    DiggCount = c.DiggCount,
                    ParentId = c.ParentId
                })
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        return ApiResult.Success(new { list, count = total });
    }

    private static async Task<ApiResult> DiggComment(
        long id,
        IRepository<CommentModel> commentRepo,
        HttpContext httpContext)
    {
        var comment = await commentRepo.GetAsync((ulong)id);
        if (comment == null) return ApiResult.Failure(Code.NotFound);
        comment.DiggCount = (comment.DiggCount ?? 0) + 1;
        commentRepo.Update(comment);
        await commentRepo.SaveChangesAsync();
        return ApiResult.Success("ok");
    }

    private static ulong? GetUserId(HttpContext ctx)
    {
        var claim = ctx.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        return ulong.TryParse(claim, out var id) ? id : null;
    }
}

public class CommentView
{
    public ulong Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Content { get; set; }
    public ulong? ArticleId { get; set; }
    public ulong? UserId { get; set; }
    public ulong? ParentId { get; set; }
    public ulong? RootParentId { get; set; }
    public long? DiggCount { get; set; }
    public string? UserNickname { get; set; }
    public string? UserAvatar { get; set; }
}

public class CreateCommentRequest
{
    public string? Content { get; set; }
    public ulong ArticleId { get; set; }
    public ulong? ParentId { get; set; }
    public ulong? RootParentId { get; set; }
}

public class CommentTreeItem
{
    public ulong Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ulong? UserId { get; set; }
    public string? Nickname { get; set; }
    public string? Avatar { get; set; }
    public long? DiggCount { get; set; }
    public long? ReplyCount { get; set; }
    public List<CommentReplyItem> Replies { get; set; } = new();
}

public class CommentReplyItem
{
    public ulong Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ulong? UserId { get; set; }
    public string? Nickname { get; set; }
    public string? Avatar { get; set; }
    public long? DiggCount { get; set; }
    public ulong? ParentId { get; set; }
}
