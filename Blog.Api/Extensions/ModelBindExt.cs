using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Extensions;

public static class ModelBindExt
{
    public static async Task<bool> BindTo<TModel>(this ControllerBase controller, TModel model) where TModel : class
    {
        if (controller.Request.ContentLength == 0)
            return false;

        using var reader = new StreamReader(controller.Request.Body);
        var body = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(body))
            return false;

        var deserializedModel = JsonSerializer.Deserialize<TModel>(body);
        if (deserializedModel == null)
            return false;

        var sourceProps = typeof(TModel).GetProperties();
        foreach (var prop in sourceProps)
        {
            var value = prop.GetValue(deserializedModel);
            prop.SetValue(model, value);
        }

        return true;
    }
}
