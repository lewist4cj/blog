using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_models")]
public class UserModel : BaseEntity
{
    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [SugarColumn(ColumnDataType = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [SugarColumn(Length = 32)]
    public string? Username { get; set; }

    [SugarColumn(Length = 32)]
    public string? Nickname { get; set; }

    [SugarColumn(Length = 64)]
    public string? Password { get; set; }

    [SugarColumn(Length = 256)]
    public string? Avatar { get; set; }

    [SugarColumn(Length = 256)]
    public string? Abstract { get; set; }

    public sbyte? RegisterSrc { get; set; }

    public long? CodeAge { get; set; }

    [SugarColumn(Length = 256)]
    public string? Email { get; set; }

    [SugarColumn(Length = 126)]
    public string? OpenId { get; set; }

    public sbyte Role { get; set; }
}
