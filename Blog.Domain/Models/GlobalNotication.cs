using SqlSugar;

namespace Blog.Domain;

[SugarTable("global_notications")]
public class GlobalNotication : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "title", Length = 32)]
    public string? Title { get; set; }

    [SugarColumn(ColumnName = "icon", Length = 256)]
    public string? Icon { get; set; }

    [SugarColumn(ColumnName = "content", Length = 64)]
    public string? Content { get; set; }

    [SugarColumn(ColumnName = "href", Length = 256)]
    public string? Href { get; set; }
}
