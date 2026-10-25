using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace DNP.MainService.TestEdgeCases.SetupTestServer.Logging;

public class KestrelRateExceptionLogger(Channel<string> events, string categoryName) : ILogger
{
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        // Capture EVERYTHING — no filter at all
        var message = $"[Category={categoryName}] [EventId={eventId.Id}] [Level={logLevel}] {formatter(state, exception)}";
        events.Writer.TryWrite(message);
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

    private sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();
        public void Dispose() { }
    }
}
