using DNP.ThisIsAMainService.Features.People.Create;

namespace DNP.ThisIsAMainService.Features.People;

public static class PeopleFeatureRegistration
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddPeopleFeature()
        {
            builder.Services
                .AddTransient<CreatePeopleCommandHandler>();

            return builder;
        }
    }

    extension(WebApplication app)
    {
        public WebApplication MapPeopleEndpoints()
        {
            var group = app.MapGroup("/people");

            group.AddCreatePersonEndpoint();

            return app;
        }
    }
}