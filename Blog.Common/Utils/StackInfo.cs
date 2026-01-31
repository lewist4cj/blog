using System.Diagnostics;
using System.Text;

namespace Blog.Common.Utils;

public class StackInfo
{
        /// <summary>
    /// 获取优化的堆栈跟踪信息
    /// </summary>
    /// <returns>格式化的堆栈跟踪字符串</returns>
    public static string GetOptimizedStackTrace()
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
    public string GetExceptionStackTrace(Exception exception, bool includeInnerExceptions = true)
    {
        var sb = new StringBuilder();
        
        AppendExceptionDetails(sb, exception, 0, includeInnerExceptions);
        
        return sb.ToString();
    }
    
    public static void AppendExceptionDetails(StringBuilder sb, Exception ex, int level, bool includeInnerExceptions)
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
    public static string GetSimplifiedStackTrace(int framesToInclude = 5)
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
    
}