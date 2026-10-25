namespace DNP.MainService.TestEdgeCases.SetupTestServer.Kestrel;

public class AdvancedKestrelOptions
{
    public double MinRequestBodyBytesPerSecond { get; set; } = 100;
    public int MinRequestBodyGracePeriodSeconds { get; set; } = 30;

    public override string ToString()
           => $"""
                The minimum data rate (in bytes/second) that the request body must maintain. It defaults to 240 bytes/second with a 5-second grace period.
                The configured is: {this.MinRequestBodyBytesPerSecond} bytes/{this.MinRequestBodyGracePeriodSeconds} seconds
            """;
}
