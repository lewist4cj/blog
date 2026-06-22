using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.Config;
using Blog.Domain.enums;
using Blog.Domain.JsonContext;
using Blog.Extensions.Validation;
using Blog.Services.ConfigMgrApp;
using Blog.Api.Extensions;
using HtmlAgilityPack;

namespace Blog.Api.Endpoints;

public static class SiteEndpoints
{
    public static RouteGroupBuilder MapSiteEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/info", GetSiteInfo).RequireAuthorization();
        group.MapGet("/redirection", GetRedirection).RequireAuthorization();
        group.MapPut("/info", UpdateSiteSetting).RequireAuthorization();
        group.MapPut("/update", UpdateSiteInfo).RequireAuthorization();
        return group;
    }

    private static async Task<ApiResult> GetSiteInfo(
        string name,
        ISiteConfigService siteConfigService,
        HttpContext httpContext)
    {
        if (name == "site")
        {
            var siteMgr = await siteConfigService.GetSiteMgrAsync();
            return ApiResult.Success(siteMgr);
        }

        // 非 site 节点需要 SuperAdmin 权限
        var userRole = GetUserRole(httpContext);
        if (userRole < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);

        var otherSiteMgr = await siteConfigService.GetOtherSiteMgrAsync();
        return name switch
        {
            "email" => ApiResult.Success(otherSiteMgr.EmailSettings!),
            "qq" => ApiResult.Success(otherSiteMgr.QqSettings!),
            "qiNiu" => ApiResult.Success(otherSiteMgr.QiNiuSettings!),
            "ai" => ApiResult.Success(otherSiteMgr.AiSettings!),
            _ => ApiResult.Failure(Code.NotFound)
        };
    }

    private static async Task<ApiResult> GetRedirection(ISiteConfigService siteConfigService)
    {
        var otherSiteMgr = await siteConfigService.GetOtherSiteMgrAsync();
        var qqSettings = otherSiteMgr.QqSettings;
        var validationResult = qqSettings!.Validate();
        if (validationResult.Code != 200)
            return validationResult;

        var url = qqSettings!.GetRedirectUrl();
        return ApiResult.Success(url!);
    }

    private static async Task<ApiResult> UpdateSiteSetting(
        string name,
        IConfiguration configuration,
        ISiteConfigService siteConfigService,
        HttpContext httpContext)
    {
        if (name == "site")
        {
            var siteMgr = await ModelBindExt.BindWithDefaults(
                httpContext.Request, new SiteMgr(configuration), DomainJsonContext.Default.SiteMgr);
            if (siteMgr == null)
                return ApiResult.Failure(Code.BadRequest, "Request body is empty or invalid");

            await siteConfigService.SaveSiteMgrAsync(siteMgr);
            return ApiResult.Success("Site settings updated and saved successfully");
        }

        // 非 site 节点需要 SuperAdmin 权限
        var userRole = GetUserRole(httpContext);
        if (userRole < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);

        var otherSiteMgr = await siteConfigService.GetOtherSiteMgrAsync();

        object? resp = name switch
        {
            "email" => await ModelBindExt.BindWithDefaults(
                httpContext.Request, otherSiteMgr.EmailSettings!, DomainJsonContext.Default.EmailSettings),
            "qq" => await ModelBindExt.BindWithDefaults(
                httpContext.Request, otherSiteMgr.QqSettings!, DomainJsonContext.Default.QqSettings),
            "qiNiu" => await ModelBindExt.BindWithDefaults(
                httpContext.Request, otherSiteMgr.QiNiuSettings!, DomainJsonContext.Default.QiNiuSettings),
            "ai" => await ModelBindExt.BindWithDefaults(
                httpContext.Request, otherSiteMgr.AiSettings!, DomainJsonContext.Default.AiSettings),
            _ => null
        };

        if (resp == null)
            return ApiResult.Failure(Code.NotFound);

        var typeName = resp.GetType().Name;
        await siteConfigService.SaveOtherSectionAsync(typeName, resp);
        return ApiResult.Success("Settings updated and saved successfully");
    }

    private static async Task<ApiResult> UpdateSiteInfo(
        SiteMgr siteMgr,
        IConfiguration configuration)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.Load("./wwwroot/uploads/index.html");
        var docNode = htmlDocument.DocumentNode;

        var titleNode = docNode.SelectSingleNode("//title");
        titleNode.InnerHtml = siteMgr.ProjectSettings!.Title!;

        var linkIcon = docNode.SelectSingleNode("//link[@rel='icon']");
        linkIcon.SetAttributeValue("href", siteMgr.ProjectSettings!.Icon!);

        var metaKeywords = docNode.SelectSingleNode("//meta[@name='keywords']");
        metaKeywords.SetAttributeValue("content", siteMgr.SeoSettings?.Keywords ?? "");

        var metaDesc = docNode.SelectSingleNode("//meta[@name='description']");
        metaDesc.SetAttributeValue("content", siteMgr.SeoSettings?.Description ?? "");

        htmlDocument.Save("./wwwroot/uploads/index.html");
        return ApiResult.Success("Settings updated and saved successfully");
    }

    private static int GetUserRole(HttpContext ctx)
    {
        var role = ctx.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        return int.TryParse(role, out var val) ? val : 0;
    }
}
