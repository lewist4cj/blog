using SqlSugar;

namespace Blog.Domain;

[SugarTable("banner_models")]
public class BannerModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "cover")]
    public string? Cover { get; set; }

    [SugarColumn(ColumnName = "href")]
    public string? Href { get; set; }
}
