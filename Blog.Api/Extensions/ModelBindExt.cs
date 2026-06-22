using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Extensions;

public static class ModelBindExt
{
    /// <summary>
    /// 读取请求体 JSON 并与默认对象深度合并，返回合并后的结果。
    /// 请求体中的属性会覆盖默认对象中的同名属性，嵌套对象递归合并。
    /// 使用 JsonTypeInfo 实现 AOT 安全的序列化。
    /// </summary>
    public static async Task<TModel?> BindWithDefaults<TModel>(
        this ControllerBase controller,
        TModel defaults,
        JsonTypeInfo<TModel> jsonTypeInfo) where TModel : class
    {
        if (controller.Request.ContentLength == 0)
            return null;

        using var reader = new StreamReader(controller.Request.Body);
        var body = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(body))
            return null;

        // 使用 AOT 安全的重载：Serialize + Deserialize 通过 JsonTypeInfo
        var defaultJson = JsonSerializer.Serialize(defaults, jsonTypeInfo);
        var defaultObj = JsonNode.Parse(defaultJson)!.AsObject();

        var requestObj = JsonNode.Parse(body)!.AsObject();

        // 深度合并：请求属性覆盖默认值
        DeepMerge(defaultObj, requestObj);

        var mergedJson = defaultObj.ToJsonString();
        return JsonSerializer.Deserialize(mergedJson, jsonTypeInfo);
    }

    private static void DeepMerge(JsonObject target, JsonObject source)
    {
        foreach (var prop in source)
        {
            if (prop.Value is JsonObject sourceObj && target[prop.Key] is JsonObject targetObj)
            {
                DeepMerge(targetObj, sourceObj);
            }
            else
            {
                target[prop.Key] = prop.Value?.DeepClone();
            }
        }
    }
}
