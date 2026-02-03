using Blog.Common.MD5Module;
using Blog.Core.Repository;
using Blog.Domain;

namespace Blog.Services.UserApp;

public class UserService(IRepository<UserModel> userRepo):IUserService
{
    public async Task<UserModel?> GetUserAsync(string username, string password)
    {
        var user = await userRepo.GetAsync(opt => opt.Username == username && opt.Password == password);
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
        var res = await userRepo
            .GetAsync(p => p.Password.Equals(md5Str) && p.Username.Equals(dto.Username));

        if (res == null)
        {
            return false;
        }

        return true;
    }
}