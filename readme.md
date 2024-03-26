# OpenTelemetry and Logging in ASP.NET Core 8

## Knowledge base

- [OpenTelemetry Docs](https://opentelemetry.io/docs/)
- [OpenTelemetry in .NET](https://opentelemetry.io/docs/languages/net/)
- [OpenTelemetry & its extensibility in .NET - GitHub](https://github.com/open-telemetry/opentelemetry-dotnet)
- [Observability in .NET](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel)
    - [Instrumentation API](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-instrumentation)
    - [Distributed Tracing API](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing-instrumentation-walkthroughs)
    - [Tracing and Instrumentation - using 'Activity Source'](https://opentelemetry.io/docs/languages/net/instrumentation/)
    - [Custom Instrumentation library](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/trace/extending-the-sdk/README.md#instrumentation-library)
- [Health Endpoint Monitoring pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/health-endpoint-monitoring)
- [Health checks in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0)
    - [HealthChecks & WatchDog](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)
    - [Resource Monitoring & K8s Probes](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostic-resource-monitoring)
- [Logging in C# and .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging)
- [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging)
- [High performance logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging)
- [Compile-time logging source generation](https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator)
- [[LogProperties] and the new telemetry logging source generator](https://andrewlock.net/behind-logproperties-and-the-new-telemetry-logging-source-generator/)
- [.NET Metrics](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics)
- [Metrics Instrumentation](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-instrumentation)
- [Types of metrics](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-instrumentation#types-of-instruments)
- [Metrics Collection](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-collection)
- [ASP.NET Core Metrics](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/built-in-metrics-aspnetcore)
- Grafana dashboards:
    - ASP.NET Core - 19924
    - ASP.NET Core Endpoint - 19925
- Tutorials:
    - [OpenTelemetry in .NET - part 1](https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-metrics-and-dotnet-part-1/)
    - [OpenTelemetry in .NET - part 2](https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-metrics-and-dotnet-part-2/)
- Diagnostics:
    - [Diagnostics client libraries](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostics-client-library)
    - [EventSource](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/eventsource)

## Tools

- [Prometheus](https://prometheus.io/)
    - [UI port: 9090](http://localhost:9090)
- [Jaeger](https://www.jaegertracing.io/)
    - [UI port: 16686](http://localhost:16686)
- [SeriLog](https://serilog.net/)
    - [SeriLog Docs](https://github.com/serilog/serilog/wiki/Getting-Started)
- [Seq](https://datalust.co/seq)
    - [UI port: 5342](http://localhost:5342)
    - [Docs](https://docs.datalust.co/docs/an-overview-of-seq)
    - [OpenTelemetry](https://docs.datalust.co/docs/opentelemetry-net-sdk)
- [Grafana](https://grafana.com/)
    - [UI port: 3000](http://localhost:3000)
- [SigNoz](https://signoz.io/)
- [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
- [.NET CLI diagnostic tools](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/tools-overview)
    - [perfview](https://github.com/microsoft/perfview)
    - [WinDbg](https://apps.microsoft.com/detail/9pgjgd53tn86?launch=true&mode=mini&hl=en-us&gl=PL)

### Configuration

Prometheus in 'prometheus.yml'

```yaml
    static_configs:
      - targets: ["localhost:9090", "localhost:5108"]
```