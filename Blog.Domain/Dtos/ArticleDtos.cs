namespace Blog.Domain.Dtos;

/// <summary>文章列表项</summary>
public class ArticleListItem
{
    public ulong Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Title { get; set; }
    public string? Abstract { get; set; }
    public ulong? CategoryId { get; set; }
    public string? CategoryTitle { get; set; }
    public string? TagList { get; set; }
    public string? Cover { get; set; }
    public ulong? UserId { get; set; }
    public string? UserNickname { get; set; }
    public string? UserAvatar { get; set; }
    public long? LookCount { get; set; }
    public long? LikeCount { get; set; }
    public long? CommentCount { get; set; }
    public long? CollectCount { get; set; }
    public bool? EnableComment { get; set; }
    public long? Status { get; set; }
    public bool? UserTop { get; set; }
    public bool? AdminTop { get; set; }
}

/// <summary>文章详情</summary>
public class ArticleDetail : ArticleListItem
{
    public string? Content { get; set; }
    public bool IsDigg { get; set; }
    public bool IsCollect { get; set; }
}

/// <summary>添加文章请求</summary>
public class ArticleAddRequest
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Abstract { get; set; }
    public long? Status { get; set; }
    public ulong? CategoryId { get; set; }
    public string? Cover { get; set; }
    public string? TagList { get; set; }
    public bool? EnableComment { get; set; }
}

/// <summary>编辑文章请求</summary>
public class ArticleEditRequest : ArticleAddRequest
{
    public ulong Id { get; set; }
}

/// <summary>分页列表响应</summary>
public class PageList<T>
{
    public List<T> List { get; set; } = new();
    public int TotalCount { get; set; }
}

/// <summary>首页选项类型</summary>
public class SelectOption
{
    public ulong? Value { get; set; }
    public string? Label { get; set; }
}

/// <summary>文章审批请求</summary>
public class ArticleExamineRequest
{
    public ulong ArticleId { get; set; }
    public long Status { get; set; }
    public string? Msg { get; set; }
}

/// <summary>收藏请求</summary>
public class ArticleCollectRequest
{
    public ulong ArticleId { get; set; }
    public ulong? CollectId { get; set; }
}

/// <summary>文章查询参数</summary>
public class ArticleQuery
{
    public long? Type { get; set; }
    public ulong? UserId { get; set; }
    public ulong? CollectId { get; set; }
    public long? Status { get; set; }
    public ulong? CategoryId { get; set; }
    public string? Key { get; set; }
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
}
