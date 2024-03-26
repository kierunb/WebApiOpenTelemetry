using Microsoft.AspNetCore.Mvc;

namespace WebApiOpenTelemetry.Controllers;

[Route("api/logging")]
[ApiController]
public partial class LoggingController(ILogger<LoggingController> logger) : ControllerBase
{
    private readonly ILogger<LoggingController> _logger = logger;

    [HttpGet]
    public IActionResult Get()
    {
        // Log levels: Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5, and None = 6.

        //_logger.LogInformation($"Logging API executed at {DateTime.UtcNow.ToLongTimeString()}"); // Wrong! CA2254 warning will be raised

        // structured logging (log message template) - basic usage
        _logger.LogInformation("Logging API executed at {DateTime}", DateTime.UtcNow.ToLongTimeString());

        // structured logging (log message template) with custom event id
        _logger.LogInformation(CustomLogEvents.WebApiInvoked, "Logging API executed at {DateTime}", DateTime.UtcNow.ToLongTimeString());

        // log scopes
        var transactionId = Guid.NewGuid().ToString();
        using (_logger.BeginScope("Transaction scope", [new KeyValuePair<string, object>("transactionId", transactionId)]))
        {
            _logger.LogInformation("Logging in transaction scope, step: {stepId}", 1);
            _logger.LogInformation("Logging in transaction scope, step: {stepId}", 2);
        }

        // API progression: LoggerMessage.Define
        _logger.FailedToExecuteLoggingController(this);

        // API progression: LoggerMessageAttribute
        GetMethodInvoked(_logger, DateTime.UtcNow.Second);

        // API progression: LoggerMessageAttribute
        LoggingExamples.UsingFormatSpecifier(_logger, 3.14);

        // API progression: LoggerMessageAttribute with [LogProperties]
        LoggingExamples.ActionMethodInvoked(_logger, new UserInfo("John Doe", 123));

        return Ok();
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Get method invoked. Number is {number}.")]
    static partial void GetMethodInvoked(ILogger logger, int number);

}

