using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_conf_models")]
[SugarIndex("uni_user_conf_models_user_id", nameof(UserId), OrderByType.Asc, true)]
public class UserConfModel : BaseEntity
{
    public ulong? UserId { get; set; }

    public string? LikeTags { get; set; }

    [SugarColumn(ColumnDataType = "timestamp(3)")]
    public DateTime? UpdateUsernameDate { get; set; }

    public bool? PublishCollections { get; set; }

    public bool? PublishFollowings { get; set; }

    public bool? PublishFans { get; set; }

    public ulong? ThemeStyleId { get; set; }
}
