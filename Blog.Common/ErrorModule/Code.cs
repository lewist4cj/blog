namespace Blog.Common;

/// <summary>
/// 预定义错误码集合
/// </summary>
public static class Code
{
    #region 客户端错误码 (4xx)
    /// <summary>
    /// 请求参数错误
    /// </summary>
    public static readonly ErrorCode BadRequest = new(400, "Bad Request");
    
    /// <summary>
    /// 未授权访问
    /// </summary>
    public static readonly ErrorCode Unauthorized = new(401, "Unauthorized");
    
    /// <summary>
    /// 禁止访问
    /// </summary>
    public static readonly ErrorCode Forbidden = new(403, "Forbidden");
    
    /// <summary>
    /// 资源未找到
    /// </summary>
    public static readonly ErrorCode NotFound = new(404, "Not Found");
    #endregion
    
    #region 服务器错误码 (5xx)
    /// <summary>
    /// 服务器内部错误
    /// </summary>
    public static readonly ErrorCode InternalServerError = new(500, "Internal Server Error");
    
    /// <summary>
    /// 功能未实现
    /// </summary>
    public static readonly ErrorCode NotImplemented = new(501, "Not Implemented");
    
    /// <summary>
    /// 服务不可用
    /// </summary>
    public static readonly ErrorCode ServiceUnavailable = new(503, "Service Unavailable");
    #endregion
    
    #region 业务错误码 (自定义范围)
    /// <summary>
    /// 用户已存在
    /// </summary>
    public static readonly ErrorCode UserAlreadyExists = new(1001, "User already exists");
    
    /// <summary>
    /// 用户名或密码错误
    /// </summary>
    public static readonly ErrorCode InvalidCredentials = new(1002, "Invalid username or password");
    
    /// <summary>
    /// 数据验证失败
    /// </summary>
    public static readonly ErrorCode ValidationFailed = new(1003, "Data validation failed");
    #endregion
}