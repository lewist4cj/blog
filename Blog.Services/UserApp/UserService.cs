using Blog.Common;
using Blog.Common.MD5Module;
using Blog.Core.Repository;
using Blog.Domain;

namespace Blog.Services.UserApp;

public class UserService(IRepository<UserModel> userRepo):IUserService
{
    public async Task<UserModel?> GetUserAsync(UserModelLoginDto dto)
    {
        var md5Str = dto.Password?.ToMD5();
        var user = await userRepo.GetAsync(opt => opt.Username!.Equals(dto.Username) && opt.Password!.Equals(md5Str));
        return user;
    }

    public async Task<UserModel?> GetUserAsync(ulong id)
    {
        var user = await userRepo.GetAsync(id);
        return user;
    }

    public async Task<bool> CheckPwd(UserModelLoginDto dto)
    {
        var md5Str = dto.Password?.ToMD5();
        Console.WriteLine("md5_str: " + md5Str);
        var res = await userRepo
            .GetAsync(p => p.Password!.Equals(md5Str) && p.Username!.Equals(dto.Username));

        if (res == null)
        {
            return false;
        }

        return true;
    }
    
   
}