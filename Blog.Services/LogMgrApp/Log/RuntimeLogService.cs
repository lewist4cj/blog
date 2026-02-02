using System.Collections;
using Blog.Common.Utils;
using Blog.Core.DbContext;
using Blog.Domain;
using blog.Models.enums.Log;
using blog.Models.enums;

using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Log;

public class RuntimeLogService(BlogDbContext context)
{
    private ArrayList ItemList  = new ArrayList();
    public string Title { get; set; } = "";
    public LogLevelEnum Level { get; set; } = LogLevelEnum.Info;
    public LogModel? LogModel { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    
    
    private void AddItem(string label, string value, LogLevelEnum level)
    {
        ItemList.Add(
            $"<div class=\"log_item {level.ToString()}\"><div class=\"log_item_label\">{label}</div><div class=\"log_item_value\">{value}</div></div>"
        );
    }
    
    public  void AddItemInfo(string label, string value)
    {
        AddItem(label, value, LogLevelEnum.Info);
    }

    public void AddItemWarning(string label, string value)
    {
        AddItem(label, value, LogLevelEnum.Warning);
    }
    /// <summary>
    ///  Add  error info into item list
    /// </summary>
    /// <param name="label"></param>
    /// <param name="value"></param>
    public void AddItemError(string label, string value)
    {
        // 同时记录堆栈信息
        var stackInfo = StackInfo.GetSimplifiedStackTrace();
        var str = $"<div class=\"log_error\"><div class=\"line\"><div class=\"label\">{label}</div><div class=\"value\">{value}</div></div><div class=\"stack\">${stackInfo}</div></div>";
        ItemList.Add(str);
    }
    
    public void AddItemNowTime()
    {
        ItemList.Add($"<div class=\"log_time\">{DateTime.Now:yyyy-M-d dddd}</div>");
    }

    public void Save(string serviceName, RunDateTimeEnum runDateTime = RunDateTimeEnum.Hour)
    {
        var logModel = new LogModel()
        {
            LogType = LogTypeEnum.RuntimeLogType,
            ServiceName = serviceName,
            Title = Title,
            Level = LogLevelEnum.Info,
            Content = string.Join("\n", ItemList.ToArray()),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        // get logs by time (使用LINQ查询替代原生SQL以提高数据库兼容性)
        var timeThreshold = GetTimeThreshold(runDateTime);
        var logs = context.LogModels
            .Where(l => l.LogType == LogTypeEnum.RuntimeLogType && l.CreatedAt >= timeThreshold)
            .OrderByDescending(l => l.UpdatedAt)
            .ToList();
        if (logs.Count > 0)
        {
            // update log
            logs[0].Content = logModel.Content;
            context.LogModels.Update(logs[0]);
            context.SaveChanges();
            ItemList.Clear();
            return;
        }
       
            // insert log
            context.LogModels.Add(logModel);
            context.SaveChanges();
            ItemList.Clear();
    }

    private DateTime GetTimeThreshold(RunDateTimeEnum runDateTime)
    {
        return runDateTime switch
        {
            RunDateTimeEnum.Hour => DateTime.Now.AddHours(-1),
            RunDateTimeEnum.Day => DateTime.Now.AddDays(-1),
            RunDateTimeEnum.Week => DateTime.Now.AddDays(-7),
            RunDateTimeEnum.Month => DateTime.Now.AddMonths(-1),
            _ => DateTime.Now.AddHours(-1)
        };
    }

    // 已废弃：原生SQL版本（仅作参考）
    /*
    private string GetLogsSqlStr(RunDateTimeEnum runDateTime)
    {
        switch (runDateTime)
        {
            case RunDateTimeEnum.Hour:
                return "INTERVAL 1 HOUR";
            case RunDateTimeEnum.Day:
                return "INTERVAL 1 DAY";
            case RunDateTimeEnum.Week:
                return "INTERVAL 1 WEEK";
            case RunDateTimeEnum.Month:
                return "INTERVAL 1 MONTH";
            default:
                return "INTERVAL 1 HOUR";
        }
    }
    */


}