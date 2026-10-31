using DNP.ThisIsAMainService.Shared.Infrastructures.HealthChecks;
using DNP.ThisIsAMainService.Shared.Infrastructures.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

builder.AddPersistence();

builder.Services
    .AddEndpointsApiExplorer()
    .AddHttpLogging(options =>
    {
        options.CombineLogs = true;
        options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    })
    .AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = context =>
        {
            context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            context.ProblemDetails.Extensions.TryAdd("traceId", Activity.Current?.Id ?? context.HttpContext.TraceIdentifier);
        };
    })
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        options.SerializerOptions.PropertyNameCaseInsensitive = true;
        options.SerializerOptions.AllowTrailingCommas = true;
        // Ignore object cycles (prevent infinite reference loops)
        options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services
    .AddControllers()
    .AddJsonOptions(json =>
    {
        json.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        json.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        json.JsonSerializerOptions.AllowTrailingCommas = true;
        // Ignore object cycles (prevent infinite reference loops)
        json.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddGrpc();

builder.Services
    .AddHealthChecks()
    .AddCustomHealthChecks();


var app = builder.Build();

app.UseExceptionHandler().UseHttpLogging();

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