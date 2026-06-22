using System.Text.Json;
using Blog.Core.Repository;
using Blog.Domain;
using Blog.Domain.Config;
using Blog.Domain.JsonContext;
using Microsoft.Extensions.Configuration;

namespace Blog.Services.ConfigMgrApp;

public class SiteConfigService : ISiteConfigService
{
    private readonly IRepository<SiteConfigModel> _configRepo;
    private readonly IConfiguration _configuration;

    public SiteConfigService(IRepository<SiteConfigModel> configRepo, IConfiguration configuration)
    {
        _configRepo = configRepo;
        _configuration = configuration;
    }

    public async Task<SiteMgr> GetSiteMgrAsync()
    {
        var dbConfig = await _configRepo.GetAsync(c => c.Section == "SiteMgr");
        if (dbConfig != null)
        {
            return JsonSerializer.Deserialize(dbConfig.ConfigValue, DomainJsonContext.Default.SiteMgr)
                   ?? new SiteMgr(_configuration);
        }

        var siteMgr = new SiteMgr(_configuration);
        await SyncToDbAsync("SiteMgr", siteMgr);
        return siteMgr;
    }

    public async Task<OtherSiteMgr> GetOtherSiteMgrAsync()
    {
        var dbConfig = await _configRepo.GetAsync(c => c.Section == "OtherSiteMgr");
        if (dbConfig != null)
        {
            return JsonSerializer.Deserialize(dbConfig.ConfigValue, DomainJsonContext.Default.OtherSiteMgr)
                   ?? new OtherSiteMgr(_configuration);
        }

        var otherMgr = new OtherSiteMgr(_configuration);
        await SyncToDbAsync("OtherSiteMgr", otherMgr);
        return otherMgr;
    }

    public async Task SaveSiteMgrAsync(SiteMgr siteMgr)
    {
        var json = JsonSerializer.Serialize(siteMgr, DomainJsonContext.Default.SiteMgr);
        await UpsertConfigAsync("SiteMgr", json);
    }

    public async Task SaveOtherSectionAsync(string sectionName, object settings)
    {
        var sectionPath = $"OtherSiteMgr:{sectionName}";
        var json = JsonSerializer.Serialize(settings, settings.GetType(), DomainJsonContext.Default);
        await UpsertConfigAsync(sectionPath, json);
    }

    private async Task SyncToDbAsync(string section, object configObj)
    {
        var json = JsonSerializer.Serialize(configObj, configObj.GetType(), DomainJsonContext.Default);
        await UpsertConfigAsync(section, json);
    }

    private async Task UpsertConfigAsync(string section, string json)
    {
        var existing = await _configRepo.GetAsync(c => c.Section == section);
        if (existing != null)
        {
            existing.ConfigValue = json;
            existing.UpdatedAt = DateTime.UtcNow;
            _configRepo.Update(existing);
        }
        else
        {
            var newConfig = new SiteConfigModel
            {
                Section = section,
                ConfigValue = json,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _configRepo.InsertAsync(newConfig);
        }

        await _configRepo.SaveChangesAsync();
    }
}
