using SqlSugar;

namespace Blog.Domain;

[SugarTable("category_models")]
public class CategoryModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(Length = 32)]
    public string? Title { get; set; }

    public ulong? UserId { get; set; }
}
