using SqlSugar;

namespace Blog.Domain;

[SugarTable("user_conf_models")]
[SugarIndex("uni_user_conf_models_user_id", nameof(UserId), OrderByType.Asc, true)]
public class UserConfModel : BaseEntity
{
    [SugarColumn(ColumnName = "user_id")]
    public ulong? UserId { get; set; }

    [SugarColumn(ColumnName = "like_tags")]
    public string? LikeTags { get; set; }

    [SugarColumn(ColumnName = "update_username_date", ColumnDataType = "datetime(3)")]
    public DateTime? UpdateUsernameDate { get; set; }

    [SugarColumn(ColumnName = "publish_collections")]
    public bool? PublishCollections { get; set; }

    [SugarColumn(ColumnName = "publish_followings")]
    public bool? PublishFollowings { get; set; }

    [SugarColumn(ColumnName = "publish_fans")]
    public bool? PublishFans { get; set; }

    [SugarColumn(ColumnName = "theme_style_id")]
    public ulong? ThemeStyleId { get; set; }
}
