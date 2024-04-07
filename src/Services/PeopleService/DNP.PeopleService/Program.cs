
using DNP.PeopleService.BackgroundServices;
using DNP.PeopleService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("default");
builder.Services.AddDbContext<DbContext, PeopleDbContext>(db =>
{
    db.UseSqlServer(connectionString, options =>
    {
        options.EnableRetryOnFailure();
        options.MigrationsAssembly(typeof(PeopleDbContext).Assembly.GetName().Name);
    });
});

builder.Services.AddHostedService<DatabaseMigrationBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();


/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory-1
/// </summary>
public partial class Program { }