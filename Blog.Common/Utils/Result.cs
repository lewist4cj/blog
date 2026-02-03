using System.Runtime.CompilerServices;

namespace Blog.Common.Utils;

/// <summary>
/// 统一API响应结果封装类
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResult
{
    /// <summary>
    /// 状态码
    /// </summary>
    public uint Code { get; set; }
    
    /// <summary>
    /// 响应消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 响应数据
    /// </summary>
    public object? Data { get; set; } = null;

    /// <summary>
    /// 构造函数 - 成功响应带数据和消息
    /// </summary>
    /// <param name="message">响应消息</param>
    /// <param name="data">响应数据</param>
    public ApiResult(string message, object data)
    {
        this.Code = 200;
        this.Message = message;
        this.Data = data;
    }
    
    public ApiResult(string message)
    {
        this.Code = 200;
        this.Message = message;
        this.Data = null;
    }
    
    /// <summary>
    /// 构造函数 - 成功响应带数据
    /// </summary>
    /// <param name="data">响应数据</param>
    public ApiResult(object data)
    {
        this.Code = 200;
        this.Message = "Success";
        this.Data = data;
    }
    
    /// <summary>
    /// 构造函数 - 错误响应
    /// </summary>
    /// <param name="code">错误码</param>
    public ApiResult(ErrorCode code)
    {
        this.Code = code.Code;
        this.Message = code.Message;
        this.Data = null;
    }
    
    /// <summary>
    /// 构造函数 - 自定义错误响应
    /// </summary>
    /// <param name="code">状态码</param>
    /// <param name="message">错误消息</param>
    /// <param name="data">错误数据</param>
    public ApiResult(uint code, string message, object data)
    {
        this.Code = code;
        this.Message = message;
        this.Data = data;
    }
    
    /// <summary>
    /// 创建成功响应
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns>成功结果</returns>
    public static ApiResult Success(object data) => new(data);

    public static ApiResult Success() => new("success");

    /// <summary>
    /// 创建失败响应
    /// </summary>
    /// <param name="code">错误码</param>
    /// <returns>失败结果</returns>
    public static ApiResult Failure(ErrorCode code) => new(code);
}