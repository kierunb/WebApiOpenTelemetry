using System.Diagnostics.Metrics;

namespace WebApiOpenTelemetry;

public class CustomMetrics
{
    private readonly IMeterFactory _meterFactory;
    private Counter<int> NumberAddedCounter { get; }
    private Counter<int> NumberSubstractedCounter { get; }
    private ObservableGauge<int> NumberGauge { get; }
    private Histogram<int> NumberHistogram { get; }

    public CustomMetrics(IMeterFactory meterFactory)
    {
        _meterFactory = meterFactory;

        var meter = _meterFactory.Create("number.operations");

        NumberAddedCounter = meter.CreateCounter<int>(name: "number.added", unit: "integer", description: "number added");
        NumberSubstractedCounter = meter.CreateCounter<int>(name: "number.substracted", unit: "integer", description: "number substracted");

        NumberHistogram = meter.CreateHistogram<int>(name: "number.histogram", unit: "integer", description: "number histogram");

    }

    public void AddNumber(int number) => NumberAddedCounter.Add(number, new KeyValuePair<string, object?>("value.type", "integer"));
    public void SubstractNumber(int number) => NumberSubstractedCounter.Add(number);

}
