using System.Runtime.CompilerServices;

namespace Blog.Common.Utils;

public class ApiResult
{

    public uint Code { get; set; }

    public string Message { get; set; }

    public object? Data { get; set; } = null;

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

    public ApiResult(object data)
    {
        this.Code = 200;
        this.Message = "Success";
        this.Data = data;
    }

    public ApiResult(ErrorCode code)
    {
        this.Code = code.Code;
        this.Message = code.Message;
        this.Data = null;
    }

    public ApiResult(uint code, string message, object data)
    {
        this.Code = code;
        this.Message = message;
        this.Data = data;
    }


    public static ApiResult Success(object data) => new(data);

    public static ApiResult Success() => new("success");

    public static ApiResult Failure(ErrorCode code) => new(code);
    public static ApiResult Failure(ErrorCode code, object errors)
    {
        return new ApiResult(code.Code, code.Message, errors!);
    }
    public static ApiResult Failure(string message, object errors)
    {
        return new ApiResult(500u, message, errors!);
    }
}