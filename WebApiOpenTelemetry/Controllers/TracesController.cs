using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApiOpenTelemetry.Services;

namespace WebApiOpenTelemetry.Controllers;

[Route("api/traces")]
[ApiController]
public class TracesController(
    ILogger<TracesController> logger,
    IHttpClientFactory httpClientFactory,
    ITestService testService) : ControllerBase
{
    private readonly ILogger<TracesController> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ITestService _testService = testService;
    private readonly ActivitySource _activitySource;

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Simple get request");
        return Ok();
    }

    [HttpGet("complex-request")]
    public async Task<IActionResult> ComplexRequest()
    {
        _logger.LogInformation("Complex get request");

        var response = await _httpClientFactory.CreateClient().GetAsync("https://jsonplaceholder.typicode.com/todos/1");
        var contentText = await response.Content.ReadAsStringAsync();

        await _testService.DoSomeWork();    // ActivitySource usage

        return Ok($"Content length: {contentText.Length}");
    }
}
