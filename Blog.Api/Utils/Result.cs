namespace blog.Utils;

public class ApiResult<T>
{
  public ApiResultCode Code { get; set; }
  
  public string Message { get; set; }
  
  public T Data { get; set; }
}

public enum ApiResultCode
{
  Success,
  Failed, 
  Empty,
}