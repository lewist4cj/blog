namespace Blog.Common.TokenModule;
public class JwtTokenModel
{
    /// <summary>
    /// 发行人
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// 听众
    /// </summary>
    public string?  Audience { get; set; } = "";

    /// <summary>
    /// 过期时间
    /// </summary>
    public int Expire { get; set; } = 10;

    /// <summary>
    /// 安全密钥
    /// </summary>
    public string?  Security { get; set; } = "";

    /// <summary>
    /// ID
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string?  UserName { get; set; }
    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }
    
    /// <summary>
    /// 用户角色
    /// </summary>
    public sbyte Role { get; set; }
}