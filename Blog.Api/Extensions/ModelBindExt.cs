using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

namespace Blog.Api.Extensions;

public static class ModelBindExt
{
    /// <summary>
    /// 读取请求体 JSON 并与默认对象深度合并，返回合并后的结果。
    /// Minimal API 版本：接受 HttpRequest 而非 ControllerBase。
    /// </summary>
    public static async Task<TModel?> BindWithDefaults<TModel>(
        HttpRequest request,
        TModel defaults,
        JsonTypeInfo<TModel> jsonTypeInfo) where TModel : class
    {
        if (request.ContentLength == 0)
            return null;

        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(body))
            return null;

        var defaultJson = JsonSerializer.Serialize(defaults, jsonTypeInfo);
        var defaultObj = JsonNode.Parse(defaultJson)!.AsObject();
        var requestObj = JsonNode.Parse(body)!.AsObject();

        DeepMerge(defaultObj, requestObj);

        return JsonSerializer.Deserialize(defaultObj.ToJsonString(), jsonTypeInfo);
    }

    private static void DeepMerge(JsonObject target, JsonObject source)
    {
        foreach (var prop in source)
        {
            if (prop.Value is JsonObject sourceObj && target[prop.Key] is JsonObject targetObj)
                DeepMerge(targetObj, sourceObj);
            else
                target[prop.Key] = prop.Value?.DeepClone();
        }
    }
}
