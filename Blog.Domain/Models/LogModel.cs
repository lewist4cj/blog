using SqlSugar;

namespace Blog.Domain;

[SugarTable("log_models")]
public class LogModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    public int? LogType { get; set; }

    [SugarColumn(Length = 32)]
    public string? Title { get; set; }

    public string? Content { get; set; }

    public int? Level { get; set; }

    public ulong? UserId { get; set; }

    [SugarColumn(Length = 32)]
    public string? Ip { get; set; }

    [SugarColumn(Length = 64)]
    public string? Addr { get; set; }

    public bool? IsRead { get; set; }

    public bool? LoginStatus { get; set; }

    [SugarColumn(ColumnName = "user_name", Length = 32)]
    public string? UserName { get; set; }

    [SugarColumn(Length = 32)]
    public string? Pwd { get; set; }

    public sbyte? LoginType { get; set; }

    [SugarColumn(Length = 32)]
    public string? ServiceName { get; set; }
}
