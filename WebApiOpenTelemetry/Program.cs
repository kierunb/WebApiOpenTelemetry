using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.ResourceMonitoring;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using WebApiOpenTelemetry;
using WebApiOpenTelemetry.HealthCheks;
using WebApiOpenTelemetry.Services;

#region Logging configuration

var seqEndpoint = "http://localhost:5341";

Log.Logger = new LoggerConfiguration()
    .Enrich.WithMachineName()
    .Enrich.WithProcessName()
    .WriteTo.Console()
    .WriteTo.Seq(seqEndpoint)
    //.WriteTo.OpenTelemetry(otlpOptions =>
    //{
    //    otlpOptions.TracesEndpoint = "http://localhost:5341/ingest/otlp/v1/logs";
    //    otlpOptions.LogsEndpoint = "http://localhost:5341/ingest/otlp/v1/logs";
    //    otlpOptions.Protocol = OtlpProtocol.HttpProtobuf;
    //})
    .CreateLogger();

#endregion

try
{
    Log.Information("Web API starting...");
    AppEventSources.Log.AppStarting("Web API starting.", DateTime.UtcNow);

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    #region DI container configuration

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHealthChecks()
        .AddCheck<SampleHealthCheck>("Sample HealthCheck");
    // https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
    //.AddNpgSql(pgConnectionString)
    //.AddRabbitMQ(rabbitConnectionString)

    builder.Services.AddResourceMonitoring();

    builder.Services.AddSingleton<CustomMetrics>();

    builder.Services.AddHttpClient();
    builder.Services.AddTransient<ITestService, TestService>();

    #endregion

    #region OpenTelemetry configuration (metrics & traces)

    var serviceName = builder.Configuration["ServiceName"]!;
    var oltpEndpoint = builder.Configuration["OtlpEndpoint"]!;
    var meterName = builder.Configuration["MeterName"]!;

    // OpenTelemetry Configuration
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(r => r.AddService(
            serviceName: serviceName,
            serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString(),
            serviceInstanceId: Environment.MachineName)
        .AddAttributes([new KeyValuePair<string, object>("environment", "development")]));

    // Tracing (and logs) Configuration
    builder.Services.AddOpenTelemetry()
        .WithTracing(builder =>
            builder
            .AddSource(serviceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            //.AddConsoleExporter()
            .AddOtlpExporter(otlpOptions => otlpOptions.Endpoint = new Uri(oltpEndpoint))); // works with Jaeger

    // Metrics Configuration
    builder.Services.AddOpenTelemetry()
      .WithMetrics(builder =>
        builder
        .AddMeter(meterName)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        //.AddConsoleExporter()
        .AddPrometheusExporter());

    // Logging Configuration - for reference
    //builder.Services.AddLogging(logging => logging.AddOpenTelemetry(openTelemetryLoggerOptions =>
    //{
    //    // Some important options to improve data quality
    //    openTelemetryLoggerOptions.IncludeScopes = true;
    //    openTelemetryLoggerOptions.IncludeFormattedMessage = true;

    //    openTelemetryLoggerOptions.AddOtlpExporter(exporter =>
    //    {
    //        // The full endpoint path is required here, when using
    //        // the `HttpProtobuf` protocol option.
    //        exporter.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
    //        exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
    //    });
    //}));

    #endregion

    var app = builder.Build();

    #region Pipeline configuration

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // disabled for testing purposes
    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    #endregion

    #region Observability endpoints

    //app.MapHealthChecks("/healthz");                    // '/healthz'
    app.MapHealthChecks("/healthz", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseOpenTelemetryPrometheusScrapingEndpoint();   // '/metrics'

    app.MapGet("/resources", ([FromServices] IResourceMonitor resourceMonitor) =>
    {
        var utilization = resourceMonitor.GetUtilization(window: TimeSpan.FromSeconds(3));
        var resources = utilization.SystemResources;

        return new
        {
            utilization.CpuUsedPercentage,
            utilization.MemoryUsedPercentage,
            utilization.MemoryUsedInBytes,
            utilization.SystemResources
        };
    }).WithName("resource-monitoring").WithOpenApi();

    #endregion

    app.Run();
    Log.Information("Web API started");
    AppEventSources.Log.AppStarting("Web API started.", DateTime.UtcNow);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


