using blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Route("api/[controller]/[action]")]
public class LogController: BaseController 
{
    public List<LogModel> GetLogLists()
    {
        return null;
    }
}