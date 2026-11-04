using DNP.ThisIsAMainService.Features.People.Create;
using DNP.ThisIsAMainService.Features.People.ImportMinimal;

namespace DNP.ThisIsAMainService.Features.People;

public static class PeopleFeatureRegistration
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddPeopleFeature()
        {
            builder.Services
                .AddTransient<CreatePeopleCommandHandler>()
                .AddTransient<ImportMinimalHandler>();
            return builder;
        }
    }

    extension(WebApplication app)
    {
        public WebApplication MapPeopleEndpoints()
        {
            var group = app.MapGroup("/people");

            group
            .AddCreatePersonEndpoint()
            .AddImportMinimalEndpoint();

            return app;
        }
    }
}