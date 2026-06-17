using Blog.Common;
using Blog.Core.Repository;
using Blog.Domain;

namespace Blog.Services.UserApp;

public class UserService(IRepository<UserModel> userRepo) : IUserService
{
    public async Task<UserModel?> GetUserAsync(UserModelLoginDto dto)
    {
        // 先查找用户（不管密码是什么哈希格式）
        var user = await userRepo.GetAsync(opt => opt.Username!.Equals(dto.Username));
        if (user == null || string.IsNullOrEmpty(user.Password))
            return null;

        // 验证密码（兼容 BCrypt 和旧版 MD5）
        if (!VerifyPassword(dto.Password!, user.Password))
            return null;

        // 如果是旧版 MD5 哈希，升级为 BCrypt
        if (IsMd5Hash(user.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password!);
            userRepo.Update(user);
            await userRepo.SaveChangesAsync();
        }

        return user;
    }

    public async Task<UserModel?> GetUserAsync(ulong id)
    {
        var user = await userRepo.GetAsync(id);
        return user;
    }

    public async Task<bool> CheckPwd(UserModelLoginDto dto)
    {
        var user = await userRepo.GetAsync(p => p.Username!.Equals(dto.Username));
        if (user == null || string.IsNullOrEmpty(user.Password))
            return false;

        return VerifyPassword(dto.Password!, user.Password);
    }

    /// <summary>
    /// 验证密码，同时支持 BCrypt 和旧版 MD5
    /// </summary>
    private static bool VerifyPassword(string inputPassword, string storedHash)
    {
        // BCrypt 哈希以 $2 开头
        if (storedHash.StartsWith("$2"))
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        // 旧版 MD5（64 位大写十六进制）
        if (IsMd5Hash(storedHash))
        {
            var md5Str = Blog.Common.MD5Module.MD5Helper.ToMD5(inputPassword);
            return string.Equals(md5Str, storedHash, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    /// <summary>
    /// 判断是否为 MD5 哈希（64 位十六进制字符串）
    /// </summary>
    private static bool IsMd5Hash(string hash)
    {
        return hash.Length == 32 && hash.All(c => Uri.IsHexDigit(c));
    }
}