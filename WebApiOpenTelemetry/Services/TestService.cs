using System.Diagnostics;

namespace WebApiOpenTelemetry.Services;

public interface ITestService
{
    Task DoSomeWork();
}

public class TestService : ITestService
{
    private readonly ILogger<TestService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ActivitySource _activitySource;

    public TestService(ILogger<TestService> logger, IConfiguration configuration)
    {

        _logger = logger;
        _configuration = configuration;
        _activitySource = new ActivitySource(_configuration["ServiceName"]!);
    }

    public async Task DoSomeWork()
    {

        using var activity = _activitySource.StartActivity("DoSomeWork");
        activity?.SetTag("operation.value", 123);
        activity?.SetTag("operation.name", "DoSomeWork!");

        _logger.LogInformation("Doing some work");
        await Task.Delay(1000);

    }


}
