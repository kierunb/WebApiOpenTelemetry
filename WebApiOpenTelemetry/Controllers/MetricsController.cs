using Microsoft.AspNetCore.Mvc;

namespace WebApiOpenTelemetry.Controllers;

[Route("api/metrics")]
[ApiController]
public class MetricsController(
    ILogger<MetricsController> logger,
    CustomMetrics customMetrics) : ControllerBase
{
    private readonly ILogger<MetricsController> _logger = logger;
    private readonly CustomMetrics _customMetrics = customMetrics;

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Metrics requested");
        return Ok();
    }

    [HttpGet]
    [Route("add")]
    public IActionResult AddNumber(int number)
    {
        _customMetrics.AddNumber(number);
        return Ok();
    }

    [HttpGet]
    [Route("sub")]
    public IActionResult SubNumber(int number)
    {
        _customMetrics.SubstractNumber(number);
        return Ok();
    }
}
