using Blog.Core.Repository;
using Blog.Domain;
using Blog.Domain.Dtos;
using SqlSugar;

namespace Blog.Services.ArticleApp;

public class ArticleService(
    ISqlSugarClient db,
    IRepository<ArticleModel> articleRepo,
    IRepository<CategoryModel> categoryRepo,
    IRepository<ArticleDiggModel> diggRepo,
    IRepository<UserArticleCollectModel> collectRepo,
    IRepository<UserArticleLookHistoryModel> historyRepo,
    IRepository<CollectModel> collectFolderRepo,
    IRepository<UserModel> userRepo
) : IArticleService
{
    public async Task<PageList<ArticleListItem>> GetArticlesAsync(ArticleQuery query)
    {
        // Base query on raw model (before Select, to allow Where on model fields)
        var q = db.Queryable<ArticleModel>()
            .LeftJoin<CategoryModel>((a, c) => a.CategoryId == c.Id)
            .LeftJoin<UserModel>((a, c, u) => a.UserId == u.Id);

        // Filters (on model fields before Select)
        if (query.Type is 1 or 2 && query.UserId.HasValue)
            q = q.Where((a, c, u) => a.UserId == query.UserId.Value);
        if (query.CategoryId.HasValue)
            q = q.Where((a, c, u) => a.CategoryId == query.CategoryId.Value);
        if (query.Status.HasValue)
            q = q.Where((a, c, u) => a.Status == query.Status.Value);
        if (!string.IsNullOrEmpty(query.Key))
            q = q.Where((a, c, u) => a.Title!.Contains(query.Key!) || a.Desc!.Contains(query.Key!));
        // collect filter via subquery
        if (query.CollectId.HasValue)
        {
            var collectId = query.CollectId.Value;
            q = q.Where((a, c, u) => SqlFunc.Subqueryable<UserArticleCollectModel>()
                .Where(col => col.ArticleId == a.Id && col.CollectId == collectId)
                .Any());
        }

        // Select after filters
        var selectQ = q.Select((a, c, u) => new ArticleListItem
        {
            Id = a.Id,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            Title = a.Title,
            Abstract = a.Desc,
            CategoryId = a.CategoryId,
            CategoryTitle = c.Title,
            TagList = a.TagList,
            Cover = a.Cover,
            UserId = a.UserId,
            UserNickname = u.Nickname,
            UserAvatar = u.Avatar,
            LookCount = a.LookCount,
            LikeCount = a.LikeCount,
            CommentCount = a.CommentCount,
            CollectCount = a.CollectCount,
            EnableComment = a.EnableComment,
            Status = a.Status,
            UserTop = a.UserTop,
            AdminTop = a.AdminTop
        });

        var total = await selectQ.CountAsync();
        var list = await selectQ.OrderByDescending(a => a.AdminTop)
            .OrderByDescending(a => a.UserTop)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((query.Page - 1) * query.Limit).Take(query.Limit)
            .ToListAsync();

        return new PageList<ArticleListItem> { List = list, TotalCount = total };
    }

    public async Task<ArticleDetail?> GetArticleAsync(ulong id, ulong? currentUserId)
    {
        var detail = await db.Queryable<ArticleModel>()
            .LeftJoin<CategoryModel>((a, c) => a.CategoryId == c.Id)
            .LeftJoin<UserModel>((a, c, u) => a.UserId == u.Id)
            .Where(a => a.Id == id)
            .Select((a, c, u) => new ArticleDetail
            {
                Id = a.Id,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                Title = a.Title,
                Abstract = a.Desc,
                Content = a.Content,
                CategoryId = a.CategoryId,
                CategoryTitle = c.Title,
                TagList = a.TagList,
                Cover = a.Cover,
                UserId = a.UserId,
                UserNickname = u.Nickname,
                UserAvatar = u.Avatar,
                LookCount = a.LookCount,
                LikeCount = a.LikeCount,
                CommentCount = a.CommentCount,
                CollectCount = a.CollectCount,
                EnableComment = a.EnableComment,
                Status = a.Status,
                UserTop = a.UserTop,
                AdminTop = a.AdminTop
            })
            .FirstAsync();

        if (detail == null) return null;

        if (currentUserId.HasValue)
        {
            detail.IsDigg = await db.Queryable<ArticleDiggModel>()
                .AnyAsync(d => d.ArticleId == id && d.UserId == currentUserId.Value);
            detail.IsCollect = await db.Queryable<UserArticleCollectModel>()
                .AnyAsync(c => c.ArticleId == id && c.UserId == currentUserId.Value);
        }

        return detail;
    }

    public async Task<ulong> CreateArticleAsync(ArticleAddRequest request, ulong userId)
    {
        var article = new ArticleModel
        {
            Title = request.Title,
            Content = request.Content,
            Desc = request.Abstract,
            CategoryId = request.CategoryId,
            Cover = request.Cover,
            TagList = request.TagList,
            EnableComment = request.EnableComment ?? true,
            Status = request.Status ?? 2,
            UserId = userId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var result = await articleRepo.InsertAsync(article);
        return result.Id;
    }

    public async Task UpdateArticleAsync(ArticleEditRequest request)
    {
        var article = await articleRepo.GetAsync(request.Id);
        if (article == null) return;
        article.Title = request.Title ?? article.Title;
        article.Content = request.Content ?? article.Content;
        article.Desc = request.Abstract ?? article.Desc;
        article.CategoryId = request.CategoryId;
        article.Cover = request.Cover ?? article.Cover;
        article.TagList = request.TagList ?? article.TagList;
        article.EnableComment = request.EnableComment ?? article.EnableComment;
        article.Status = request.Status ?? article.Status;
        article.UpdatedAt = DateTime.Now;
        articleRepo.Update(article);
        await articleRepo.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(ulong id)
    {
        var article = await articleRepo.GetAsync(id);
        if (article != null)
        {
            articleRepo.Delete(article);
            await articleRepo.SaveChangesAsync();
        }
    }

    public async Task ExamineArticleAsync(ArticleExamineRequest request)
    {
        var article = await articleRepo.GetAsync(request.ArticleId);
        if (article == null) return;
        article.Status = request.Status;
        article.UpdatedAt = DateTime.Now;
        articleRepo.Update(article);
        await articleRepo.SaveChangesAsync();
    }

    public async Task<bool> ToggleDiggAsync(ulong articleId, ulong userId)
    {
        var existing = await db.Queryable<ArticleDiggModel>()
            .FirstAsync(d => d.ArticleId == articleId && d.UserId == userId);
        var article = await articleRepo.GetAsync(articleId);
        if (article == null) return false;

        if (existing != null)
        {
            db.Deleteable(existing).ExecuteCommand();
            article.LikeCount = Math.Max(0, (article.LikeCount ?? 0) - 1);
        }
        else
        {
            await db.Insertable(new ArticleDiggModel
            {
                ArticleId = articleId,
                UserId = userId,
                CreatedAt = DateTime.Now
            }).ExecuteCommandAsync();
            article.LikeCount = (article.LikeCount ?? 0) + 1;
        }

        articleRepo.Update(article);
        await articleRepo.SaveChangesAsync();
        return existing == null;
    }

    public async Task CollectArticleAsync(ulong articleId, ulong? collectId, ulong userId)
    {
        var existing = await db.Queryable<UserArticleCollectModel>()
            .FirstAsync(c => c.ArticleId == articleId && c.UserId == userId);
        if (existing != null)
        {
            db.Deleteable(existing).ExecuteCommand();
            await AdjustCollectCount(articleId, -1);
        }
        else
        {
            await db.Insertable(new UserArticleCollectModel
            {
                ArticleId = articleId,
                CollectId = collectId ?? 0,
                UserId = userId,
                CreatedAt = DateTime.Now
            }).ExecuteCommandAsync();
            await AdjustCollectCount(articleId, 1);
        }
    }

    public async Task RemoveArticleCollectAsync(ulong collectId, List<ulong> articleIds, ulong userId)
    {
        foreach (var articleId in articleIds)
        {
            var existing = await db.Queryable<UserArticleCollectModel>()
                .FirstAsync(c => c.ArticleId == articleId && c.CollectId == collectId && c.UserId == userId);
            if (existing != null)
            {
                db.Deleteable(existing).ExecuteCommand();
            }
        }
    }

    public async Task RecordHistoryAsync(ulong articleId, ulong userId)
    {
        var existing = await db.Queryable<UserArticleLookHistoryModel>()
            .FirstAsync(h => h.ArticleId == articleId && h.UserId == userId);
        if (existing != null)
        {
            existing.CreatedAt = DateTime.Now;
            db.Updateable(existing).ExecuteCommand();
        }
        else
        {
            await db.Insertable(new UserArticleLookHistoryModel
            {
                ArticleId = articleId,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }).ExecuteCommandAsync();
        }

        // increment look count
        var article = await articleRepo.GetAsync(articleId);
        if (article != null)
        {
            article.LookCount = (article.LookCount ?? 0) + 1;
            articleRepo.Update(article);
            await articleRepo.SaveChangesAsync();
        }
    }

    public async Task<PageList<ArticleListItem>> GetHistoryAsync(int type, string? key, int page, int limit, ulong? userId)
    {
        var q = db.Queryable<UserArticleLookHistoryModel>()
            .Where(h => h.UserId == userId)
            .LeftJoin<ArticleModel>((h, a) => h.ArticleId == a.Id)
            .LeftJoin<UserModel>((h, a, u) => a.UserId == u.Id)
            .Select((h, a, u) => new ArticleListItem
            {
                Id = a.Id,
                Title = a.Title,
                Abstract = a.Desc,
                Cover = a.Cover,
                UserId = a.UserId,
                UserNickname = u.Nickname,
                UserAvatar = u.Avatar,
                LookCount = a.LookCount,
                LikeCount = a.LikeCount,
                CommentCount = a.CommentCount,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            });

        var total = await q.CountAsync();
        var list = await q.OrderByDescending(h => h.CreatedAt)
            .Skip((page - 1) * limit).Take(limit).ToListAsync();

        return new PageList<ArticleListItem> { List = list, TotalCount = total };
    }

    public async Task DeleteHistoryAsync(List<ulong> idList, ulong userId)
    {
        foreach (var id in idList)
        {
            var h = await historyRepo.GetAsync(id);
            if (h != null && h.UserId == userId)
            {
                historyRepo.Delete(h);
            }
        }
        await historyRepo.SaveChangesAsync();
    }

    public async Task ToggleTopAsync(ulong articleId, int type, ulong userId)
    {
        var article = await articleRepo.GetAsync(articleId);
        if (article == null) return;
        if (type == 1) article.AdminTop = !article.AdminTop;
        else article.UserTop = !article.UserTop;
        articleRepo.Update(article);
        await articleRepo.SaveChangesAsync();
    }

    public async Task<List<SelectOption>> GetTagOptionsAsync()
    {
        var tags = await db.Queryable<ArticleModel>()
            .Where(a => a.TagList != null)
            .Select(a => a.TagList!)
            .ToListAsync();

        var tagSet = new HashSet<string>();
        foreach (var t in tags)
        {
            foreach (var tag in t.Split(',', StringSplitOptions.RemoveEmptyEntries))
                tagSet.Add(tag.Trim());
        }
        return tagSet.Select(t => new SelectOption { Label = t }).ToList();
    }

    public async Task<List<ArticleListItem>> GetRecommendAsync(ulong? userId)
    {
        return await db.Queryable<ArticleModel>()
            .Where(a => a.Status == 3)
            .LeftJoin<UserModel>((a, u) => a.UserId == u.Id)
            .Select((a, u) => new ArticleListItem
            {
                Id = a.Id,
                Title = a.Title,
                Abstract = a.Desc,
                Cover = a.Cover,
                LookCount = a.LookCount,
                LikeCount = a.LikeCount,
                CommentCount = a.CommentCount,
                UserNickname = u.Nickname,
                UserAvatar = u.Avatar
            })
            .OrderByDescending(a => a.LookCount)
            .Take(10).ToListAsync();
    }

    private async Task AdjustCollectCount(ulong articleId, int delta)
    {
        var article = await articleRepo.GetAsync(articleId);
        if (article != null)
        {
            article.CollectCount = Math.Max(0, (article.CollectCount ?? 0) + delta);
            articleRepo.Update(article);
            await articleRepo.SaveChangesAsync();
        }
    }
}
