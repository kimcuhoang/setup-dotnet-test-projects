using DNP.MainService.TestEdgeCases.SetupTestServer.Kestrel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;

namespace DNP.MainService.TestEdgeCases.SetupTestServer.Kestrel;

public static class KestrelRegistrar
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder ConfigureKestrel(int port)
        {

            builder.WebHost.ConfigureKestrel((context, options) =>
            {
                var configuration = context.Configuration;

                options.AddServerHeader = false;

                AdvancedKestrelOptions advancedKestrelOptions = new();
                configuration.GetRequiredSection(nameof(AdvancedKestrelOptions)).Bind(advancedKestrelOptions);

                options.Limits.MinRequestBodyDataRate = new MinDataRate
                (
                    bytesPerSecond: advancedKestrelOptions.MinRequestBodyBytesPerSecond,
                    gracePeriod: TimeSpan.FromSeconds(advancedKestrelOptions.MinRequestBodyGracePeriodSeconds)
                );

                options.ListenLocalhost(port);
            });

            return builder;
        }
    }
}
