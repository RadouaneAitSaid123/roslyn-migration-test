using System.Diagnostics;

namespace Dotnet6MultiPattern.Services;

public class TelemetryService
{
    public void TrackOperation(string operationName)
    {
        Console.WriteLine($"[Telemetry] Tracking operation: {operationName}");

        var activity = new Activity(operationName);
        activity.Start();

        try
        {
            Thread.Sleep(100);

            using var nested = new Activity("nested-op");
            nested.Start();
            nested.AddTag("source", "legacy");

            Console.WriteLine("[Telemetry] Nested op done.");
        }
        finally
        {
            activity.Stop();
        }
    }
}
