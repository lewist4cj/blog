using System.Text.Json.Serialization;
using Blog.Common.RespModule;
using Blog.Common.Utils;
using Blog.Domain.Config;
using Blog.Domain.Dtos;

namespace Blog.Domain.JsonContext;

[JsonSerializable(typeof(ApiResult))]
[JsonSerializable(typeof(PageResult<LogModel>))]
[JsonSerializable(typeof(LogModel))]
[JsonSerializable(typeof(UserModelLoginDto))]
[JsonSerializable(typeof(SiteMgr))]
[JsonSerializable(typeof(OtherSiteMgr))]
[JsonSerializable(typeof(SiteSettings))]
[JsonSerializable(typeof(AboutSettings))]
[JsonSerializable(typeof(ArticleSettings))]
[JsonSerializable(typeof(SeoSettings))]
[JsonSerializable(typeof(ComponentSettings))]
[JsonSerializable(typeof(LoginSettings))]
[JsonSerializable(typeof(ProjectSettings))]
[JsonSerializable(typeof(EmailSettings))]
[JsonSerializable(typeof(AiSettings))]
[JsonSerializable(typeof(QqSettings))]
[JsonSerializable(typeof(QiNiuSettings))]
// Article
[JsonSerializable(typeof(ArticleListItem))]
[JsonSerializable(typeof(ArticleDetail))]
[JsonSerializable(typeof(ArticleAddRequest))]
[JsonSerializable(typeof(ArticleEditRequest))]
[JsonSerializable(typeof(ArticleExamineRequest))]
[JsonSerializable(typeof(ArticleCollectRequest))]
[JsonSerializable(typeof(ArticleQuery))]
[JsonSerializable(typeof(SelectOption))]
public partial class DomainJsonContext : JsonSerializerContext
{
}
