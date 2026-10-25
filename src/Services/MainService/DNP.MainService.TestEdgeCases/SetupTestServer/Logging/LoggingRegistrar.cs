
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace DNP.MainService.TestEdgeCases.SetupTestServer.Logging;

public static class LoggingRegistrar
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddCustomLogging()
        {
            builder.Services.AddLogging(log =>
            {
                log.ClearProviders();                    // remove all existing providers
                log.SetMinimumLevel(LogLevel.Trace);     // capture everything
                log.AddFilter("Microsoft.AspNetCore.Server.Kestrel.BadRequests", LogLevel.Trace);
                log.AddFilter("Microsoft.AspNetCore.Http.RequestDelegateFactory", LogLevel.Trace);
                log.Services.AddSingleton<ILoggerProvider, KestrelRateLoggerProvider>();
            });


            return builder;
        }
    }
}
