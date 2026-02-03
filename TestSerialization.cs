using System.Text.Json;
using Blog.Common.Utils;

// 测试ApiResult序列化是否符合要求的格式
Console.WriteLine("=== ApiResult序列化测试 ===");

// 测试失败情况
var failureResult = ApiResult.Failure(new ErrorCode(400, "用户名或密码错误"));
var failureJson = JsonSerializer.Serialize(failureResult, new JsonSerializerOptions 
{ 
    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    WriteIndented = false
});

Console.WriteLine("失败响应:");
Console.WriteLine(failureJson);

// 测试成功情况
var successResult = ApiResult.Success(new { username = "testuser", token = "sample_token" });
var successJson = JsonSerializer.Serialize(successResult, new JsonSerializerOptions 
{ 
    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    WriteIndented = false
});

Console.WriteLine("\n成功响应:");
Console.WriteLine(successJson);

// 测试空数据情况
var emptyResult = ApiResult.Success();
var emptyJson = JsonSerializer.Serialize(emptyResult, new JsonSerializerOptions 
{ 
    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    WriteIndented = false
});

Console.WriteLine("\n空数据响应:");
Console.WriteLine(emptyJson);