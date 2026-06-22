using Blog.Common;
using Blog.Common.Utils;
using Blog.Domain.Config;
using Blog.Domain.enums;
using Blog.Api.Extensions;
using Blog.Extensions.Validation;
using Blog.Services.ConfigMgrApp;
using Blog.Domain.JsonContext;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
public class SiteController(IConfiguration configuration, ISiteConfigService siteConfigService) : BaseController
{
    [HttpGet("info")]
    public async Task<ApiResult> SiteInfo(string name)
    {

        if (name == "site")
        {
            var siteMgr = await siteConfigService.GetSiteMgrAsync();
            return ApiResult.Success(siteMgr);
        }

        // check the role of the user. if the user is not admin, return failure.
        var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        _ = int.TryParse(role, out int userRoleValue);
        if (userRoleValue < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);
        var otherSiteMgr = await siteConfigService.GetOtherSiteMgrAsync();
        switch (name)
        {
            case "email":
                var email = otherSiteMgr.EmailSettings!;
                return ApiResult.Success(email);
            case "qq":
                var qqSettings = otherSiteMgr.QqSettings!;
                return ApiResult.Success(qqSettings);
            case "qiNiu":
                var beiAn = otherSiteMgr.QiNiuSettings!;
                return ApiResult.Success(beiAn);
            case "ai":
                var ai = otherSiteMgr.AiSettings!;
                return ApiResult.Success(ai);
            default:
                return ApiResult.Failure(Code.NotFound);
        }
    }

    [HttpGet("redirection")]
    public async Task<ApiResult> Redirection()
    {
        var otherSiteMgr = await siteConfigService.GetOtherSiteMgrAsync();
        var qqSettings = otherSiteMgr.QqSettings;
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
            var siteMgr = await this.BindWithDefaults(new SiteMgr(configuration), DomainJsonContext.Default.SiteMgr);
            if (siteMgr == null)
            {
                return ApiResult.Failure(Code.BadRequest, "Request body is empty or invalid");
            }

            await siteConfigService.SaveSiteMgrAsync(siteMgr);
            return ApiResult.Success("Site settings updated and saved successfully");
        }

        // check the user role
        var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        _ = int.TryParse(role, out int userRoleValue);
        if (userRoleValue < (int)RoleEnum.SuperAdmin)
            return ApiResult.Failure(Code.Forbidden);
        var otherSiteMgr = await siteConfigService.GetOtherSiteMgrAsync();

        object? resp = null;
        switch (name)
        {
            case "email":
                otherSiteMgr.EmailSettings = await this.BindWithDefaults(otherSiteMgr.EmailSettings!, DomainJsonContext.Default.EmailSettings);
                resp = otherSiteMgr.EmailSettings!;
                break;
            case "qq":
                otherSiteMgr.QqSettings = await this.BindWithDefaults(otherSiteMgr.QqSettings!, DomainJsonContext.Default.QqSettings);
                resp = otherSiteMgr.QqSettings!;
                break;
            case "qiNiu":
                otherSiteMgr.QiNiuSettings = await this.BindWithDefaults(otherSiteMgr.QiNiuSettings!, DomainJsonContext.Default.QiNiuSettings);
                resp = otherSiteMgr.QiNiuSettings!;
                break;
            case "ai":
                otherSiteMgr.AiSettings = await this.BindWithDefaults(otherSiteMgr.AiSettings!, DomainJsonContext.Default.AiSettings);
                resp = otherSiteMgr.AiSettings!;
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
                await siteConfigService.SaveOtherSectionAsync(typeName, resp);
                return ApiResult.Success("Settings updated and saved successfully");
            default:
                return ApiResult.Failure(Code.NotFound);
        }

    }

    [HttpPut("update")]
    public ApiResult UpdateSiteInfo([FromBody]SiteMgr siteMgr)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.Load("./wwwroot/uploads/index.html");
        var docNode = htmlDocument.DocumentNode;

        var titleNode = docNode.SelectSingleNode("//title");
        if (titleNode != null)
            titleNode.InnerHtml = siteMgr.ProjectSettings!.Title!;

        var linkIcon = docNode.SelectSingleNode("//link[@rel='icon']");
        if (linkIcon == null)
        {
            var htmlNode = htmlDocument.CreateElement("link");
            htmlNode.SetAttributeValue("rel", "icon");
            htmlNode.SetAttributeValue("href", siteMgr.ProjectSettings!.Icon!);
            docNode.ChildNodes.Add(htmlNode);
        }
        else
        {
            linkIcon.SetAttributeValue("href", siteMgr.ProjectSettings!.Icon!);
        }

        var metaKeywords = docNode.SelectSingleNode("//meta[@name='keywords']");
        if (metaKeywords == null)
        {
            var htmlNode = htmlDocument.CreateElement("meta");
            htmlNode.SetAttributeValue("name", "keywords");
            htmlNode.SetAttributeValue("content", siteMgr.SeoSettings?.Keywords ?? "");
            docNode.ChildNodes.Add(htmlNode);
        }
        else
        {
            metaKeywords.SetAttributeValue("content", siteMgr.SeoSettings?.Keywords ?? "");
        }

        var metaDesc = docNode.SelectSingleNode("//meta[@name='description']");
        if (metaDesc == null)
        {
            var htmlNode = htmlDocument.CreateElement("meta");
            htmlNode.SetAttributeValue("name", "description");
            htmlNode.SetAttributeValue("content", siteMgr.SeoSettings?.Description ?? "");
            docNode.ChildNodes.Add(htmlNode);
        }
        else
        {
            metaDesc.SetAttributeValue("content", siteMgr.SeoSettings?.Description ?? "");
        }

        htmlDocument.Save("./wwwroot/uploads/index.html");

        return ApiResult.Success("Settings updated and saved successfully");
    }

}

