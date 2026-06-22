using SqlSugar;

namespace Blog.Domain;

[SugarTable("log_models")]
public class LogModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "log_type")]
    public int? LogType { get; set; }

    [SugarColumn(ColumnName = "title", Length = 32)]
    public string? Title { get; set; }

    [SugarColumn(ColumnName = "content")]
    public string? Content { get; set; }

    [SugarColumn(ColumnName = "level")]
    public int? Level { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }

    [SugarColumn(ColumnName = "ip", Length = 32)]
    public string? Ip { get; set; }

    [SugarColumn(ColumnName = "addr", Length = 64)]
    public string? Addr { get; set; }

    [SugarColumn(ColumnName = "is_read")]
    public bool? IsRead { get; set; }

    [SugarColumn(ColumnName = "login_status")]
    public bool? LoginStatus { get; set; }

    [SugarColumn(ColumnName = "user_name", Length = 32)]
    public string? UserName { get; set; }

    [SugarColumn(ColumnName = "pwd", Length = 32)]
    public string? Pwd { get; set; }

    [SugarColumn(ColumnName = "login_type")]
    public sbyte? LoginType { get; set; }

    [SugarColumn(ColumnName = "service_name", Length = 32)]
    public string? ServiceName { get; set; }
}
