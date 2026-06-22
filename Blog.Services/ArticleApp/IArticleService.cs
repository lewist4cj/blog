using Blog.Domain.Dtos;

namespace Blog.Services.ArticleApp;

public interface IArticleService
{
    Task<PageList<ArticleListItem>> GetArticlesAsync(ArticleQuery query);
    Task<ArticleDetail?> GetArticleAsync(ulong id, ulong? currentUserId);
    Task<ulong> CreateArticleAsync(ArticleAddRequest request, ulong userId);
    Task UpdateArticleAsync(ArticleEditRequest request);
    Task DeleteArticleAsync(ulong id);
    Task ExamineArticleAsync(ArticleExamineRequest request);
    Task<bool> ToggleDiggAsync(ulong articleId, ulong userId);
    Task CollectArticleAsync(ulong articleId, ulong? collectId, ulong userId);
    Task RemoveArticleCollectAsync(ulong collectId, List<ulong> articleIds, ulong userId);
    Task RecordHistoryAsync(ulong articleId, ulong userId);
    Task<PageList<ArticleListItem>> GetHistoryAsync(int type, string? key, int page, int limit, ulong? userId);
    Task DeleteHistoryAsync(List<ulong> idList, ulong userId);
    Task ToggleTopAsync(ulong articleId, int type, ulong userId);
    Task<List<SelectOption>> GetTagOptionsAsync();
    Task<List<ArticleListItem>> GetRecommendAsync(ulong? userId);
}
