using SqlSugar;

namespace Blog.Domain;

[SugarTable("global_notications")]
public class GlobalNotication : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(Length = 32)]
    public string? Title { get; set; }

    [SugarColumn(Length = 256)]
    public string? Icon { get; set; }

    [SugarColumn(Length = 64)]
    public string? Content { get; set; }

    [SugarColumn(Length = 256)]
    public string? Href { get; set; }
}
