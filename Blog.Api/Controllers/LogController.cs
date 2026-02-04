using Blog.Core.Repository;
using Blog.Domain;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers;

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