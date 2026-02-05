using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Common.TokenModule.Models;
using Blog.Extensions.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Common.TokenModule;

public static class TokenHepler
{
    public static string GenerateToken(JwtTokenModel? jwtTokenModel)
    {
        if (jwtTokenModel == null) 
            return "JwtTokenModel object is null"; 

        // 验证配置
        jwtTokenModel.ValidateOrThrow("JWT Token Configuration");

        var claims = new List<Claim>
        {
            new Claim("Id", jwtTokenModel.Id.ToString()),
            new Claim("UserName", jwtTokenModel.UserName ?? ""),
            new Claim("NickName", jwtTokenModel.NickName ?? ""),
            new Claim("Role", (jwtTokenModel.Role?.ToString() ?? "0"))
        };

        
        // 生成密钥
        var credentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenModel.Security!));
        var signingCredential = new SigningCredentials(credentials, SecurityAlgorithms.HmacSha256);

        // 生成token
        var token = new JwtSecurityToken(
            issuer: jwtTokenModel.Issuer,
            audience: jwtTokenModel.Audience,
            expires: DateTime.Now.AddHours(jwtTokenModel.Expires),
            signingCredentials: signingCredential,
            claims: claims
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }

     public static SecurityToken GetSecurityToken(string token)
    {
        var jwtSection = AppSettings.Configuration!.GetSection("Jwt");
        var tokenModel = jwtSection.Get<JwtTokenModel>()!;
        tokenModel.ValidateAndReturn();
        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters parameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenModel.Issuer,
            ValidAudience = tokenModel.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenModel.Security!))
        };
        tokenHandler.ValidateToken(token, parameters, out var securityToken);
        return securityToken;

    }
}