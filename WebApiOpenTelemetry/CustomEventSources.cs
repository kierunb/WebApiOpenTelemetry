using System.Diagnostics.Tracing;

namespace WebApiOpenTelemetry;

[EventSource(Name = "WebApiOpenTelemetry")]
public class AppEventSources : EventSource
{
    public static AppEventSources Log { get; } = new AppEventSources();

    [Event(1)]
    public void AppStarting(string message, DateTime dateTime) => WriteEvent(1, message, dateTime);

    [Event(2)]
    public void AppStarted(string message, DateTime dateTime) => WriteEvent(2, message, dateTime);
}
