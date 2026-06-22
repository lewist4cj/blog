using System.Text.Json.Serialization;
using Blog.Common.RespModule;
using Blog.Common.Utils;
using Blog.Domain.Config;
using Blog.Domain.Dtos;

namespace Blog.Domain.JsonContext;

// 开发调试时使用 DefaultJsonTypeInfoResolver（反射），无需手动注册类型。
// 以下注册仅 AOT 发布时需要——如遇缺失类型请在此补充。
[JsonSerializable(typeof(ApiResult))]
[JsonSerializable(typeof(PageResult<LogModel>))]
[JsonSerializable(typeof(PageList<ArticleListItem>))]
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
[JsonSerializable(typeof(List<SelectOption>))]
public partial class DomainJsonContext : JsonSerializerContext
{
}
