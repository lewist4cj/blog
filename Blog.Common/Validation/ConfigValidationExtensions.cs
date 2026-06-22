using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Blog.Common.Utils;
using Microsoft.Extensions.Configuration;

namespace Blog.Extensions.Validation;

public static class ConfigValidationExtensions
{
    /// <summary>
    /// validate the configuration object from configuration section, if validation fails, throw an exception
    /// </summary>
    /// <typeparam name="T">configuration type</typeparam>
    /// <param name="configuration">IConfiguration instance</param>
    /// <param name="sectionName">configuration section name</param>
    /// <returns>验证后的配置对象</returns>
    [RequiresUnreferencedCode("Config types are preserved via IOptions binding at startup")]
    public static T GetValidatedConfig<T>(this IConfiguration configuration, string sectionName)
     where T : class, IValidatableObject, new()
    {
        var config = configuration.GetSection(sectionName).Get<T>();

        if (config == null)
        {
            throw new InvalidOperationException($"Configuration section '{sectionName}' is missing from app settings.");
        }

        var validationContext = new ValidationContext(config);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(config, validationContext, validationResults, true);

        if (!isValid)
        {
            var errorMessage = string.Join("; ", validationResults.Where(vr => vr != null).Select(vr => vr.ErrorMessage));
            throw new InvalidOperationException($"Configuration validation for '{sectionName}' failed: {errorMessage}");
        }

        return config;
    }

    /// <summary>
    /// 验证配置对象，如果验证失败则抛出异常
    /// </summary>
    /// <param name="obj">要验证的对象</param>
    /// <param name="configName">配置名称，用于错误消息</param>
    public static void ValidateOrThrow(this IValidatableObject obj, string configName = "Configuration")
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj), $"{configName} object is null");

        var validationContext = new ValidationContext(obj);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        if (!isValid)
        {
            var errorMessage = string.Join("; ", validationResults.Where(vr => vr != null).Select(vr => vr.ErrorMessage));
            throw new InvalidOperationException($"{configName} validation failed: {errorMessage}");
        }
    }

    /// <summary>
    /// 验证配置对象，如果验证失败则返回错误的 ApiResult
    /// </summary>
    /// <param name="obj">要验证的对象</param>
    /// <param name="configName">配置名称，用于错误消息</param>
    /// <returns>验证成功返回配置对象，失败返回错误信息</returns>
    public static ApiResult Validate(this IValidatableObject obj, string configName = "Configuration")
    {
        if (obj == null)
        {
            var errorMessage = $"{configName} object is null";
            return ApiResult.Failure(Common.Code.BadRequest, errorMessage);
        }

        var validationContext = new ValidationContext(obj);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        if (!isValid)
        {
            var errorMessage = string.Join("; ", validationResults.Where(vr => vr != null).Select(vr => vr.ErrorMessage));
            var errors = validationResults
                .Where(vr => vr != null)
                .Select(vr => new { Field = vr.MemberNames.FirstOrDefault(), Message = vr.ErrorMessage })
                .ToArray();

            return ApiResult.Failure(Common.Code.BadRequest, errors);
        }

        return ApiResult.Success(obj);
    }
}
