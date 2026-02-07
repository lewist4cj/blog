using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.enums;
using Blog.Extensions;
using Blog.Extensions.Config;
using Blog.Extensions.Validation;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace blog.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
public class SiteController(ILogger<SiteController> _logger) : BaseController
{
    [HttpGet("info")]
    public ApiResult SiteInfo(string name)
    {

        if (name == "site")
        {
            var siteMgr = AppSettings.Configuration!.GetSection("SiteMgr").Get<SiteMgr>()!;
            return ApiResult.Success(siteMgr);
        }


        // check the role of the user. if the user is not admin, return failure.
        var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        _ = int.TryParse(role, out int userRoleValue);
        if (userRoleValue < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);
        var otherSiteMgr = AppSettings.Configuration!.GetSection("OtherSiteMgr").Get<OtherSiteMgr>()!;
        switch (name)
        {
            case "email":
                var email = otherSiteMgr.emailSettings!;
                return ApiResult.Success(email);
            case "qq":
                var qqSettings = otherSiteMgr.qqSettings!;
                return ApiResult.Success(qqSettings);
            case "qiNiu":
                var beiAn = otherSiteMgr.qiNiuSettings!;
                return ApiResult.Success(beiAn);
            case "ai":
                var ai = otherSiteMgr.aiSettings!;
                return ApiResult.Success(ai);
            default:
                return ApiResult.Failure(Code.NotFound);
        }
    }

    [HttpGet("redirection")]
    public ApiResult Redirection()
    {
        var otherSiteMgr = AppSettings.Configuration!.GetSection("OtherSiteMgr").Get<OtherSiteMgr>()!;
        var qqSettings = otherSiteMgr.qqSettings;
        var validationResult = qqSettings!.Validate();

        if (validationResult.Code != 200)
        {
            return validationResult;
        }

        var url = qqSettings!.GetRedirectUrl();
        return ApiResult.Success(url!);
    }

    [HttpPut("info")]
    public async Task<ApiResult> UpdateSiteSetting(string name)
    {
        if (name == "site")
        {
            var siteMgr = new SiteMgr();

            if (!await this.BindTo(siteMgr))
            {
                return ApiResult.Failure(Code.BadRequest, "Request body is empty or invalid");
            }

            var result = await SaveConfigToFile("SiteMgr", siteMgr);
            if (!result)
            {
                return ApiResult.Failure(Code.InternalServerError, "Failed to save settings to configuration file");
            }

            return ApiResult.Success("Site settings updated and saved successfully");
        }

        // check the user role
        var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        _ = int.TryParse(role, out int userRoleValue);
        if (userRoleValue < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);
        var otherSiteMgr = AppSettings.Configuration!.GetSection("OtherSiteMgr").Get<OtherSiteMgr>()!;

        object? resp = null;
        switch (name)
        {
            case "email":
                await this.BindTo(otherSiteMgr.emailSettings!);
                resp = otherSiteMgr.emailSettings!;
                break;
            case "qq":
                await this.BindTo(otherSiteMgr.qqSettings!);
                resp = otherSiteMgr.emailSettings!;
                break;
            case "qiNiu":
                await this.BindTo(otherSiteMgr.qiNiuSettings!);
                resp = otherSiteMgr.emailSettings!;
                break;
            case "ai":
                await this.BindTo(otherSiteMgr.aiSettings!);
                resp = otherSiteMgr.aiSettings!;
                break;
            default:
                return ApiResult.Failure(Code.NotFound);
        }

        switch (name)
        {
            case "email":
            case "qq":
            case "qiNiu":
            case "ai":
                var typeName = resp.GetType().Name;
                var sectionPath = $"OtherSiteMgr:{typeName}";
                await SaveConfigToFile(sectionPath, resp);
                return ApiResult.Success("Settings updated and saved successfully");
            default:
                return ApiResult.Failure(Code.NotFound);
        }

    }

    [HttpPut("update")]
    public ApiResult UpdateSiteInfo(SiteMgr siteMgr)
    {
        var exists = System.IO.File.Exists(siteMgr.ProjectSettings!.Fontend);
        if (!exists)
        {
            _logger.LogError($"File not exists {siteMgr.ProjectSettings.Fontend}");
            return ApiResult.Failure(Code.FileNotExists);
        }

        var htmlDocument = new HtmlDocument();
        htmlDocument.Load("./wwwroot/uploads/index.html");

        var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//title");
        titleNode.InnerHtml = siteMgr.ProjectSettings.Title!;
        var linkIcon = htmlDocument.DocumentNode.SelectSingleNode("//link[@rel='icon']");
        if (linkIcon == null)
        {
            HtmlNode htmlNode = htmlDocument.CreateElement("link");
            htmlNode.SetAttributeValue("rel", "icon");
            htmlNode.SetAttributeValue("href", siteMgr.ProjectSettings.Icon!);

        }
        linkIcon?.SetAttributeValue("href", siteMgr.ProjectSettings.Icon!);
        var metaKeywords = htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='keywords']");
        if (metaKeywords == null)
        {
            HtmlNode htmlNode = htmlDocument.CreateElement("meta");
            htmlNode.SetAttributeValue("name", "keywords");
            htmlNode.SetAttributeValue("content", siteMgr.SeoSettings?.Keywords!);

        }
        metaKeywords?.SetAttributeValue("content", siteMgr.SeoSettings?.Keywords!);
        var metaDesc = htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='description']");
        if (metaDesc == null)
        {
            HtmlNode htmlNode = htmlDocument.CreateElement("meta");
            htmlNode.SetAttributeValue("name", "description");
            htmlNode.SetAttributeValue("content", siteMgr.SeoSettings?.Description!);

        }
        metaDesc?.SetAttributeValue("content", siteMgr.SeoSettings?.Keywords!);

        // save data into the html file. 
        htmlDocument.Save(siteMgr.ProjectSettings.Fontend!);

        return ApiResult.Success("Settings updated and saved successfully");
    }

    private async Task<bool> SaveConfigToFile(string sectionPath, object settingValue)
    {
        try
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configPath = string.IsNullOrEmpty(environment)
                ? "appsettings.json"
                : $"appsettings.{environment}.json";

            var pathParts = sectionPath.Split(':');
            if (pathParts.Length == 0) return false;

            var jsonContent = await System.IO.File.ReadAllTextAsync(configPath);
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent) ?? [];

            // Check if settingValue is a complex type (i.e., not a simple type such as string, int, bool, etc.)
            if (IsComplexType(settingValue))
            {
                var complexObjJson = JsonSerializer.Serialize(settingValue);
                var complexObjDict = JsonSerializer.Deserialize<Dictionary<string, object>>(complexObjJson) ?? [];

                NavigateAndSetValue(jsonObject, pathParts, complexObjDict);
            }
            else
            {
                NavigateAndSetValue(jsonObject, pathParts, settingValue);
            }

            // Serialize and write to a file
            // 
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,  // 这个选项确保输出格式化为多行
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            var updatedJson = JsonSerializer.Serialize(jsonObject, options);

            await System.IO.File.WriteAllTextAsync(configPath, updatedJson);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving config: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 判断对象是否为复杂类型（非简单类型）
    /// </summary>
    /// <param name="obj">要判断的对象</param>
    /// <returns>如果是复杂类型返回true，否则返回false</returns>
    private static bool IsComplexType(object obj)
    {
        if (obj == null) return false;

        var type = obj.GetType();

        // 简单类型包括：基本数据类型和它们的可空版本
        if (type.IsPrimitive ||
            type == typeof(string) ||
            type == typeof(DateTime) ||
            type == typeof(DateTimeOffset) ||
            type == typeof(TimeSpan) ||
            type == typeof(Guid) ||
            type == typeof(decimal) ||
            type.IsEnum ||
            Nullable.GetUnderlyingType(type) != null) // 可空类型
        {
            return false;
        }

        // 处理常见集合类型
        if (type.IsArray ||
            type.GetInterface("IEnumerable") != null &&
            type != typeof(string))
        {
            // 对于集合，检查元素是否为简单类型
            return !IsSimpleCollection(obj);
        }

        // 其他类型认为是复杂类型
        return true;
    }

    /// <summary>
    /// 判断是否为简单类型的集合
    /// </summary>
    /// <param name="collection">要检查的集合</param>
    /// <returns>如果是简单类型集合返回true，否则返回false</returns>
    private static bool IsSimpleCollection(object collection)
    {
        if (collection == null) return true;

        var enumerable = collection as System.Collections.IEnumerable;
        if (enumerable == null) return true;

        foreach (var item in enumerable)
        {
            if (item == null) continue;

            var itemType = item.GetType();
            if (itemType.IsPrimitive ||
                itemType == typeof(string) ||
                itemType == typeof(DateTime) ||
                itemType == typeof(DateTimeOffset) ||
                itemType == typeof(TimeSpan) ||
                itemType == typeof(Guid) ||
                itemType == typeof(decimal) ||
                itemType.IsEnum ||
                Nullable.GetUnderlyingType(itemType) != null)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void NavigateAndSetValue(IDictionary<string, object> dict, string[] pathParts, object value)
    {
        // 遍历路径直到倒数第二个部分
        for (int i = 0; i < pathParts.Length - 1; i++)
        {
            var key = pathParts[i];
            if (!dict!.ContainsKey(key))
            {
                // 如果路径不存在，创建新对象
                var newDict = new Dictionary<string, object>();
                dict[key] = newDict;
                dict = newDict;
            }
            else
            {
                // 如果路径存在且是JsonElement，需要转换为字典
                if (dict[key] is JsonElement element)
                {
                    var jsonString = element.GetRawText();
                    var subDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                    dict[key] = subDict!;
                    dict = subDict!;
                }
                else if (dict[key] is Dictionary<string, object> subDict)
                {
                    dict = subDict;
                }
                else
                {
                    // 如果路径存在但不是对象，替换为新对象
                    var newDict = new Dictionary<string, object>();
                    dict[key] = newDict;
                    dict = newDict;
                }
            }
        }

        // 设置最后一个路径部分的值
        dict[pathParts[^1]] = value;
    }
}

