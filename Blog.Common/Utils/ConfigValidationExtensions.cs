using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Blog.Common.Utils;

public static class ConfigValidationExtensions
{
    /// <summary>
    /// validate the configuration object from configuration section, if validation fails, throw an exception
    /// </summary>
    /// <typeparam name="T">configuration type</typeparam>
    /// <param name="configuration">IConfiguration instance</param>
    /// <param name="sectionName">configuration section name</param>
    /// <returns>验证后的配置对象</returns>
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
    /// valiate configuration object, if validation fails, throw an exception
    /// </summary>
    /// <typeparam name="T">configuration type</typeparam>
    /// <param name="obj">target the obj</param>
    /// <param name="configName">configuration name, used to be error messages</param>
    public static T ValidateAndReturn<T>(this T obj, string configName = "Configuration") where T : IValidatableObject
    {
        obj.ValidateOrThrow(configName);
        return obj;
    }
}