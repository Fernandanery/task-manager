using Microsoft.AspNetCore.Mvc;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.WebApi.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is working! 🚀");
    }
}
