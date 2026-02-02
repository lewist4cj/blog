using Blog.Core.Repository;
using Blog.Domain;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;
[Route("api/[controller]/[action]")]
public class LogController(IRepository<LogModel> repository): BaseController 
{
    public List<LogModel> GetLogLists()
    {
       return repository.GetList();
    }

    public void UpdateLog()
    {
        
    }
}