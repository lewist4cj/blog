using System.Text.Json.Serialization;
using Blog.Api.Endpoints;
using Blog.Domain.JsonContext;

namespace Blog.Api.JsonContext;

/// <summary>
/// API 层的 JSON 序列化上下文（补充 DomainJsonContext）
/// </summary>
[JsonSerializable(typeof(CategoryView))]
[JsonSerializable(typeof(CommentView))]
[JsonSerializable(typeof(CommentTreeItem))]
[JsonSerializable(typeof(CommentReplyItem))]
[JsonSerializable(typeof(CreateCommentRequest))]
[JsonSerializable(typeof(CollectView))]
[JsonSerializable(typeof(CreateCollectRequest))]
[JsonSerializable(typeof(CreateCategoryRequest))]
[JsonSerializable(typeof(DeleteRequest))]
[JsonSerializable(typeof(RemoveCollectRequest))]
[JsonSerializable(typeof(HistoryRecordRequest))]
[JsonSerializable(typeof(DeleteHistoryRequest))]
[JsonSerializable(typeof(TopArticleRequest))]
public partial class AppJsonContext : JsonSerializerContext
{
}
