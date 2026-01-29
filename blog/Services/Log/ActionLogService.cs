using System.Collections;
using System.Diagnostics;
using System.Text;
using blog.Models;
using blog.Models.enums.Log;

namespace blog.Services.Log;

public class ActionLogService
{
    private ActionLogService()
    {
        
    }

    public LogLevelEnum Level { get; set; } = LogLevelEnum.Info;

    public string Title { get; set; }  
    
    public string RequestBody { get; set; }
    
    public string ResponseBody { get; set; }

    public bool ShowRequest { get; set; }
    
    public bool ShowResponse { get; set; }

    public ArrayList ItemList { get; set; } = [];

    public bool IsMiddleware { get; set; }
    
    public LogModel? LogModel { get; set; } = null;

    /// <summary>
    /// 获取优化的堆栈跟踪信息
    /// </summary>
    /// <returns>格式化的堆栈跟踪字符串</returns>
    private string GetOptimizedStackTrace()
    {
        var stackTrace = new StackTrace(true);
        var frames = stackTrace.GetFrames();
        
        var sb = new StringBuilder();
        sb.AppendLine("详细堆栈跟踪信息:");
        sb.AppendLine("====================");
        
        foreach (var frame in frames)
        {
            var method = frame.GetMethod();
            if (method != null)
            {
                // 过滤掉系统内部方法，只显示用户代码
                var fileName = frame.GetFileName();
                var lineNumber = frame.GetFileLineNumber();
                
                sb.AppendFormat("  在 {0}.{1}", method.DeclaringType?.FullName ?? "Unknown", method.Name);
                
                if (!string.IsNullOrEmpty(fileName))
                {
                    sb.AppendFormat(" 文件: {0} 行号: {1}", fileName, lineNumber == 0 ? "N/A" : lineNumber.ToString());
                }
                
                sb.AppendLine();
            }
        }
        
        return sb.ToString();
    }
    
    /// <summary>
    /// 从异常获取堆栈跟踪信息
    /// </summary>
    /// <param name="exception">要分析的异常</param>
    /// <param name="includeInnerExceptions">是否包含内部异常</param>
    /// <returns>格式化的异常和堆栈跟踪信息</returns>
    private string GetExceptionStackTrace(Exception exception, bool includeInnerExceptions = true)
    {
        var sb = new StringBuilder();
        
        AppendExceptionDetails(sb, exception, 0, includeInnerExceptions);
        
        return sb.ToString();
    }
    
    private void AppendExceptionDetails(StringBuilder sb, Exception ex, int level, bool includeInnerExceptions)
    {
        var indent = new string(' ', level * 4);
        
        sb.AppendFormat("{0}异常类型: {1}\n", indent, ex.GetType().FullName);
        sb.AppendFormat("{0}异常消息: {1}\n", indent, ex.Message);
        
        if (!string.IsNullOrEmpty(ex.StackTrace))
        {
            sb.AppendFormat("{0}堆栈跟踪:\n", indent);
            
            var stackLines = ex.StackTrace.Split('\n');
            foreach (var line in stackLines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    sb.AppendFormat("{0}{1}  {2}\n", indent, new string(' ', 2), line.Trim());
                }
            }
        }
        
        if (includeInnerExceptions && ex.InnerException != null)
        {
            sb.AppendLine();
            sb.AppendFormat("{0}内部异常:\n", indent);
            AppendExceptionDetails(sb, ex.InnerException, level + 1, true);
        }
    }
    
    /// <summary>
    /// 获取简化的调用堆栈信息（仅关键帧）
    /// </summary>
    /// <param name="framesToInclude">要包含的帧数，默认为5个</param>
    /// <returns>简化的堆栈跟踪</returns>
    private string GetSimplifiedStackTrace(int framesToInclude = 5)
    {
        var stackTrace = new StackTrace(true);
        var frames = stackTrace.GetFrames();
        
        var sb = new StringBuilder();
        sb.AppendLine("简化堆栈跟踪:");
        sb.AppendLine("================");
        
        var count = 0;
        foreach (var frame in frames)
        {
            if (count >= framesToInclude) break;
            
            var method = frame.GetMethod();
            if (method != null)
            {
                // 只包含用户代码（有文件名和行号的帧）
                var fileName = frame.GetFileName();
                var lineNumber = frame.GetFileLineNumber();
                
                if (!string.IsNullOrEmpty(fileName) && lineNumber > 0)
                {
                    sb.AppendFormat("  #{0} {1}.{2}() 在 {3}:第 {4} 行\n", 
                        count, 
                        method.DeclaringType?.Name ?? "Anonymous", 
                        method.Name, 
                        System.IO.Path.GetFileName(fileName), 
                        lineNumber);
                    count++;
                }
            }
        }
        
        if (count == 0)
        {
            // 如果没有找到源码位置，则显示所有帧但限制数量
            count = 0;
            foreach (var frame in frames)
            {
                if (count >= framesToInclude) break;
                
                var method = frame.GetMethod();
                if (method != null)
                {
                    sb.AppendFormat("  #{0} {1}.{2}()\n", 
                        count, 
                        method.DeclaringType?.Name ?? "Anonymous", 
                        method.Name);
                    count++;
                }
            }
        }
        
        return sb.ToString();
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
        var stackInfo = GetSimplifiedStackTrace();
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
    }
    
    public static ActionLogService GetActionLogService(HttpContext context)
    {
        var log = context.Items["log"];
        if (log == null)
        {
            return new ActionLogService();
        }

        context.Items["saved"] = true;
        return (ActionLogService)log;
    }

    public long Insert(HttpContext ctx)
    {
        var _itemList = new ArrayList();
        if (this.LogModel != null)
        {
          var newContent =  string.Join("\n", ItemList.ToArray());
          LogModel.Content = newContent + "\n" + LogModel.Content;
          // TODO: update the field LogModel
          
          ItemList.Clear();
          return LogModel.Id;
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
        var model  = new LogModel
        {
            Id = 0,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            LogType = LogTypeEnum.ActionLogType,
            Title = "Action Log",
            Content = string.Join("\n", _itemList.ToArray()),
            Level = LogLevelEnum.Info,
            UserId = 0,
            Ip = ctx.Connection.RemoteIpAddress.ToString(),
            Addr = "0.0.0.0",
            IsRead = false,
        };
        // TODO insert the model record into database
        LogModel = model;
        ItemList.Clear();
        return model.Id;
    }
    
    
}