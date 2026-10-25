
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace DNP.MainService.TestEdgeCases.SetupTestServer.Logging;

public class KestrelRateLoggerProvider(Channel<string> events) : ILoggerProvider
{
    private readonly Channel<string> _events = events;

    public ILogger CreateLogger(string categoryName)
        => new KestrelRateExceptionLogger(_events, categoryName);

    public void Dispose() { }
}
