using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.WebApi.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/[controller]")]
public class TestController(ApplicationDbContext context, ILogger<TestController> logger) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<TestController> _logger = logger;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is working! 🚀");
    }

    [Authorize]
    [HttpGet("long-operation")]
    public async Task<IActionResult> TesteLongo(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando operação longa...");

            await Task.Delay(10000, cancellationToken);

            _logger.LogInformation("Operação concluída com sucesso.");
            return Ok("Operação concluída.");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operação foi cancelada pelo cliente.");
            return StatusCode(499, "Cancelado pelo cliente");
        }
    }
}
