using SqlSugar;

namespace Blog.Domain;

[SugarTable("collect_models")]
public class CollectModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(Length = 32)]
    public string? Title { get; set; }

    [SugarColumn(Length = 256)]
    public string? Abstract { get; set; }

    [SugarColumn(Length = 256)]
    public string? Cover { get; set; }

    public long? ArticleCount { get; set; }

    public ulong? UserId { get; set; }
}
