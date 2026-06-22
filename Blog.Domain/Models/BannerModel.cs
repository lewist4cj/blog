using SqlSugar;

namespace Blog.Domain;

[SugarTable("banner_models")]
public class BannerModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    public string? Cover { get; set; }

    public string? Href { get; set; }
}
