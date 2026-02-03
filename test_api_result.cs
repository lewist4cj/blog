using System.Text.Json;
using Blog.Common.Utils;

// 测试ApiResult序列化
var result = ApiResult.Failure(new ErrorCode(400, "用户名或密码错误"));
var json = JsonSerializer.Serialize(result, new JsonSerializerOptions 
{ 
    WriteIndented = true,
    PropertyNamingPolicy = null
});

Console.WriteLine("序列化结果:");
Console.WriteLine(json);

// 测试成功的ApiResult
var successResult = ApiResult.Success(new { username = "testuser", token = "sample_token" });
var successJson = JsonSerializer.Serialize(successResult, new JsonSerializerOptions 
{ 
    WriteIndented = true,
    PropertyNamingPolicy = null
});

Console.WriteLine("\n成功响应序列化结果:");
Console.WriteLine(successJson);