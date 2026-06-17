
using blog.Controllers;
using Blog.Common;
using Blog.Common.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class UploadsController : BaseController
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".zip", ".rar", ".pdf", ".doc", ".docx" };
    private const long MaxFileSize = 50 * 1024 * 1024; // 50MB

    [HttpPost("upload")]
    public async Task<ApiResult> Upload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return ApiResult.Failure(Code.BadRequest, "No file uploaded or file is empty");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            return ApiResult.Failure(Code.BadRequest, $"File type '{extension}' is not allowed");

        if (file.Length > MaxFileSize)
            return ApiResult.Failure(Code.BadRequest, $"File size exceeds the limit of {MaxFileSize / 1024 / 1024}MB");

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return ApiResult.Success(new { fileName, size = file.Length });
    }
}