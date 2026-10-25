using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace DNP.ThisIsAMainService.Infrastructures.Kestrel;

public static class KestrelRegistrar
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder ConfigureKestrel()
        {
            //builder.Services
            //        .AddOptions<AdvancedKestrelOptions>()
            //        .BindConfiguration(nameof(AdvancedKestrelOptions));

            builder.WebHost.ConfigureKestrel((context, options) =>
            {
                var configuration = context.Configuration;

                options.AddServerHeader = false;
                options.Configure(configuration.GetSection("Kestrel"));

                //options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB


                AdvancedKestrelOptions advancedKestrelOptions = new();
                configuration.GetRequiredSection(nameof(AdvancedKestrelOptions)).Bind(advancedKestrelOptions);

                options.Limits.MinRequestBodyDataRate = new MinDataRate
                (
                    bytesPerSecond: advancedKestrelOptions.MinRequestBodyBytesPerSecond,
                    gracePeriod: TimeSpan.FromSeconds(advancedKestrelOptions.MinRequestBodyGracePeriodSeconds)
                );

            });

            return builder;
        }
    }
}
