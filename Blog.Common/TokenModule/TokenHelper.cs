using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Common.TokenModule;

public static class TokenHepler
{
    public static string GenerateToken(JwtTokenModel? jwtTokenModel)
    {
        if (jwtTokenModel == null) 
            return "JwtTokenModel object is null"; 
        var id = jwtTokenModel.Id;
        var username = jwtTokenModel.UserName??"";
        // var nickname = jwtTokenModel.NickName ?? "";
        var role = jwtTokenModel.Role;
        var claims = new[]{
            new Claim("Id",id.ToString()),
            new Claim("UserName",username!),
            new Claim("Role", role.ToString())
        };

        // 生成密钥
        var credentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenModel.Security!));
        var signingCredential = new SigningCredentials(credentials, SecurityAlgorithms.HmacSha256);

        // 生成token
        var token = new JwtSecurityToken(
            issuer: jwtTokenModel.Issuer,
            audience: jwtTokenModel.Audience,
            expires: DateTime.Now.AddMinutes(jwtTokenModel.Expire),
            signingCredentials: signingCredential,
            claims: claims
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }
    
    
}