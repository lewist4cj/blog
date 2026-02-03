using Blog.Domain;

namespace Blog.Services.UserApp;

public interface IUserService:ITag
{
    Task<UserModel?> GetUserAsync(string username, string password);
    Task<UserModel?> GetUserAsync(ulong id);
    Task<bool> CheckPwd(UserModelLoginDto dto);
}