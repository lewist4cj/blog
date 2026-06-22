using System.Text.Json.Serialization;
using Blog.Api.Endpoints;

namespace Blog.Api.JsonContext;

/// <summary>
/// API 层的 JSON 序列化上下文（补充 DomainJsonContext）
/// 开发调试时使用反射（DefaultJsonTypeInfoResolver），无需注册。
/// 以下注册仅 AOT 发布时需要——如遇缺失类型请在此补充。
/// </summary>
[JsonSerializable(typeof(CategoryView))]
[JsonSerializable(typeof(CategoryListResult))]
[JsonSerializable(typeof(List<CategoryView>))]
[JsonSerializable(typeof(CommentView))]
[JsonSerializable(typeof(CommentTreeItem))]
[JsonSerializable(typeof(CommentReplyItem))]
[JsonSerializable(typeof(CreateCommentRequest))]
[JsonSerializable(typeof(CollectView))]
[JsonSerializable(typeof(CreateCollectRequest))]
[JsonSerializable(typeof(CreateCategoryRequest))]
[JsonSerializable(typeof(DeleteRequest))]
[JsonSerializable(typeof(RemoveCollectRequest))]
[JsonSerializable(typeof(CollectDeleteRequest))]
[JsonSerializable(typeof(HistoryRecordRequest))]
[JsonSerializable(typeof(DeleteHistoryRequest))]
[JsonSerializable(typeof(TopArticleRequest))]
public partial class AppJsonContext : JsonSerializerContext
{
}
