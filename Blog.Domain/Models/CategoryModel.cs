using SqlSugar;

namespace Blog.Domain;

[SugarTable("category_models")]
public class CategoryModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "title", Length = 32)]
    public string? Title { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }
}
