namespace blog.Models.Log;

public class RuntimeLog
{
    public DateTime Timestamp { get; set; }
    
    public string Level { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;
    
    public string StackTrace { get; set; } = string.Empty;
    
    public string ContextInfo { get; set; } = string.Empty;
    
    public string MachineName { get; set; } = string.Empty;
    
    public string ApplicationName { get; set; } = string.Empty;
}