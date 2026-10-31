using DNP.ThisIsAMainService.Features.People.Domain;

namespace DNP.ThisIsAMainService.Features.People;

internal static class PeopleFeatureRegistration
{
    public static WebApplicationBuilder AddPeopleFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        return builder;
    }
}
