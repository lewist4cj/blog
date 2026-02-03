using System.Security.Cryptography;
using System.Text;

namespace Blog.Common.MD5Module;

public static class MD5Helper
{
    public static string ToMD5(this string origin)
    {
        var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.Default.GetBytes("lewist4cj"));
        var str = BitConverter.ToString(bytes).Replace("-", "");
        return str;
    }
}