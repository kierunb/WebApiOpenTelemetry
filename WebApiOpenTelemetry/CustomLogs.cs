using WebApiOpenTelemetry.Controllers;

namespace WebApiOpenTelemetry;

// LoggerMessage.Define 
// https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging
public static class CustomLogs
{
    // High-performance logging
    // https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging

    private static readonly Action<ILogger, Exception> failedToExecuteLoggingController =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(13, nameof(LoggingController)),
            "Epic failure processing item!");

    public static void FailedToExecuteLoggingController(this ILogger logger, LoggingController loggingController) => failedToExecuteLoggingController(logger, default!);
}


// LoggerMessageAttribute
// https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator
public partial class LoggingExamples
{
    private readonly ILogger _logger;

    public LoggingExamples(ILogger logger)
    {
        _logger = logger;
    }

    [LoggerMessage(
        EventId = 20,
        Level = LogLevel.Critical,
        Message = "Value is {Value:E}")]
    public static partial void UsingFormatSpecifier(ILogger logger, double value);

    [LoggerMessage(
        EventId = 9,
        Level = LogLevel.Trace,
        Message = "Fixed message",
        EventName = "CustomEventName")]
    public partial void LogWithCustomEventName();

    [LoggerMessage(
        EventId = 10,
        Message = "Welcome to {City} {Province}!")]
    public partial void LogWithDynamicLogLevel(string city, LogLevel level, string province);

    [LoggerMessage(
        EventId = 11,
        Level = LogLevel.Information,
        Message = "Action method invoked")]
    public static partial void ActionMethodInvoked(ILogger logger, [LogProperties] UserInfo userInfo);
}

public record UserInfo(string Name, int UserId);
