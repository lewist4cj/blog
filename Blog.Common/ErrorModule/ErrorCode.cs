namespace Blog.Common;

/// <summary>
/// 错误码定义类
/// </summary>
/// <param name="Code">HTTP状态码</param>
/// <param name="Message">错误消息</param>
public record ErrorCode(uint Code, string Message)
{
    /// <summary>
    /// 隐式转换为uint
    /// </summary>
    public static implicit operator uint(ErrorCode errorCode) => errorCode.Code;
    
    /// <summary>
    /// 隐式转换为string
    /// </summary>
    public static implicit operator string(ErrorCode errorCode) => errorCode.Message;
    
    /// <summary>
    /// 重写ToString方法
    /// </summary>
    public override string ToString() => $"{Code}: {Message}";
}


