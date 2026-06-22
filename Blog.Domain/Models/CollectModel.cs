using SqlSugar;

namespace Blog.Domain;

[SugarTable("collect_models")]
public class CollectModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "title", Length = 32)]
    public string? Title { get; set; }

    [SugarColumn(ColumnName = "abstract", Length = 256)]
    public string? Abstract { get; set; }

    [SugarColumn(ColumnName = "cover", Length = 256)]
    public string? Cover { get; set; }

    [SugarColumn(ColumnName = "article_count")]
    public long? ArticleCount { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }
}
