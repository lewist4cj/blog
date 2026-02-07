
using blog.Controllers;
using Blog.Common.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;
public class UploadsController : BaseController
{
   [HttpPost("upload")]
   public ApiResult Upload(IFormFile file)
    {
        
        return  ApiResult.Success("Uploaded successfully.");
    }
}