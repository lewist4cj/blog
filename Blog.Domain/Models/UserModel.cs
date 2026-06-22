using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_models")]
public class UserModel : BaseEntity
{
    [SugarColumn(ColumnName = "created_at", ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnName = "updated_at", ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(ColumnName = "username", Length = 32)]
    public string? Username { get; set; }

    [SugarColumn(ColumnName = "nickname", Length = 32)]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "password", Length = 64)]
    public string? Password { get; set; }

    [SugarColumn(ColumnName = "avatar", Length = 256)]
    public string? Avatar { get; set; }

    [SugarColumn(ColumnName = "abstract", Length = 256)]
    public string? Abstract { get; set; }

    [SugarColumn(ColumnName = "register_src")]
    public sbyte? RegisterSrc { get; set; }

    [SugarColumn(ColumnName = "code_age")]
    public long? CodeAge { get; set; }

    [SugarColumn(ColumnName = "email", Length = 256)]
    public string? Email { get; set; }

    [SugarColumn(ColumnName = "open_id", Length = 126)]
    public string? OpenId { get; set; }

    [SugarColumn(ColumnName = "role")]
    public sbyte Role { get; set; }
}
