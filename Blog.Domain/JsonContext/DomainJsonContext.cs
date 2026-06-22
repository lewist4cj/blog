using System.Text.Json.Serialization;
using Blog.Common.Utils;
using Blog.Domain.Config;

namespace Blog.Domain.JsonContext;

/// <summary>
/// AOT 安全的 JSON 序列化上下文（源码生成）
/// 所有在 AOT 模式下需要序列化的类型必须在此注册
/// </summary>
[JsonSerializable(typeof(ApiResult))]
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
public partial class DomainJsonContext : JsonSerializerContext
{
}
