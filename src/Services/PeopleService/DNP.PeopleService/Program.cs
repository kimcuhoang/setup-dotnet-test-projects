using DNP.PeopleService.Features.People;
using DNP.PeopleService.Infrastructures.HealthChecks;
using DNP.PeopleService.Infrastructures.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddPersistence()
    .AddPeopleFeature();

builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers()
    .AddJsonOptions(json =>
    {
        json.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        json.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        json.JsonSerializerOptions.AllowTrailingCommas = true;
    });

builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

builder.Services
    .AddHealthChecks()
    .AddCustomHealthChecks();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = HealthCheckRegistration.WriteResponse,
});

await app.RunAsync();


/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory-1
/// </summary>
public partial class Program { }