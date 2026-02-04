using Blog.Common;
using Blog.Domain;

namespace Blog.Services.UserApp;

public interface IUserService:ITag
{
    Task<UserModel?> GetUserAsync(UserModelLoginDto dto);
    Task<UserModel?> GetUserAsync(ulong id);
    Task<bool> CheckPwd(UserModelLoginDto dto);
}