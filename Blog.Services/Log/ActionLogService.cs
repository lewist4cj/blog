using System.Collections;
using blog.Models.enums.Log;
using Blog.Services.Local;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Blog.Common.Utils;
using Blog.Core.DbContext;
using Blog.Models;

namespace Blog.Services.Log;

public class ActionLogService
{
    private readonly BlogDbContext? _context;
    private readonly LocalService _localService;

    public ActionLogService()
    {
        
    }
    public ActionLogService(BlogDbContext context, LocalService localService)
    {
        _context = context;
        _localService = localService;
    }

    public LogLevelEnum Level { get; set; } = LogLevelEnum.Info;

    public string Title { get; set; } = "";

    public string RequestBody { get; set; } = "";
    
    public string ResponseBody { get; set; } = "";

    public bool ShowRequest { get; set; } = false;

    public bool ShowResponse { get; set; } = false;

    public ArrayList ItemList { get; set; } = [];

    public bool IsMiddleware { get; set; } = false;
    
    public LogModel? LogModel { get; set; } = null;
    
    public static ActionLogService GetActionLogService(HttpContext context)
    {
        var log = context.Items["log"];
        if (log == null)
        {
            // 遵循 HttpContext 使用规范：通过 RequestServices 获取服务实例
            return context.RequestServices.GetRequiredService<ActionLogService>();
        }
        

        context.Items["saved"] = true;
        return (ActionLogService)log;
    }

    public ActionLogService GetLog(HttpContext ctx)
    {
        if (ctx.Items["log"] is not  ActionLogService log)
        {
           return new ActionLogService();
        }
        ctx.Items["saved"] = true;
        return log; 
    }


    private void AddItem(string label, string value, LogLevelEnum level)
    {
        ItemList.Add(
            $"<div class=\"log_item {level}\"><div class=\"log_item_label\">{label}</div><div class=\"log_item_value\">${value}</div></div>"
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
        var str =
            $"<div class=\"log_error\"><div class=\"line\"><div class=\"label\">{label}</div><div class=\"value\">${value}</div></div><div class=\"stack\">${stackInfo}</div></div>";
        ItemList.Add(str);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    public void IsMiddlewareSave(HttpContext httpContext)
    {
        var saved = httpContext.Items["saved"];
        if (saved == null)
        {
            return;
        }

        if (this.LogModel != null)
        {
            this.IsMiddleware = true;
        }
        
        // setting response 
        if (this.ShowResponse)
        {
            this.ItemList.Add(
                $"<div class=\"log_response\"><pre class=\"log_json_body\">{this.ResponseBody}</pre></div>");
        }
        // TODO insert the record into database
        if (_context != null)
        {
            _context.LogModels.Add(LogModel!);
            _context.SaveChanges();
        }
    }
    
    public long Insert(HttpContext ctx)
    {
        var _itemList = new ArrayList();
        if (this.LogModel != null)
        {
          var newContent =  string.Join("\n", ItemList.ToArray());
          LogModel.Content = newContent + "\n" + LogModel.Content;
          // TODO: update the field LogModel
          if (_context != null)
          {
              _context.LogModels.Update(LogModel);
              _context.SaveChanges();
          }
          ItemList.Clear();
          return (long)LogModel.Id;
        }
        
        // setting  request info 
        if (this.ShowRequest)
        {
            _itemList.Add(
                $"<div class=\"log_request\"><div class=\"log_request_header\"><div class=\"log_request_method\">{ctx.Request.Method}</div><div class=\"log_request_path\">{ctx.Request.Path}</div></div><div class=\"log_request_params\">{ctx.Request.QueryString}</div><div class=\"log_json_body\">{ResponseBody}</div></div>");
        }
        
        // setting content list 
        foreach (var item in ItemList)
        {
            _itemList.Add(item);
        }
        
        // if the LogModel field is not None, it has been saved.
        if (IsMiddleware)
        {
            if (ShowResponse)
            {
                _itemList.Add($"<div class=\"log_response\"><pre class=\"log_json_body\">{ResponseBody}</pre></div>");
            }
        }
        
        // setting item list
        var ip = ctx.Connection.RemoteIpAddress.ToString() ?? "unknow";
        var local = LocalService.GetLocalByIp(ip);
        // 截断过长的地址信息，避免数据库字段长度限制
        var truncatedAddr = local?.Length > 64 ? local.Substring(0, 64) : local;
        var model  = new LogModel
        {
            // Id由数据库自动生成，不需要手动设置
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            LogType = LogTypeEnum.ActionLogType,
            Title = "Action Log",
            Content = string.Join("\n", _itemList.ToArray()),
            Level = LogLevelEnum.Info,
            UserId = 0,
            Ip = ip,
            Addr = truncatedAddr,
            IsRead = false,
        };
        // TODO insert the model record into database
        // 遵循依赖注入空值防护规范：使用前检查 _context 是否为 null
        if (_context != null)
        {
            Console.WriteLine($"log list content is {model.Content}");
            _context.LogModels.Add(model);
            _context.SaveChanges();
        }
        else
        {
            throw new InvalidOperationException("DbContext 未正确注入，请检查依赖注入配置");
        }
        LogModel = model;
        ItemList.Clear();
        return (long)model.Id;
    }
    
    
}