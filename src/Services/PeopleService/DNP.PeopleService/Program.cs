
using DNP.PeopleService.BackgroundServices;
using DNP.PeopleService.Domain;
using DNP.PeopleService.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers()
    .AddJsonOptions(json =>
    {
        json.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        json.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        json.JsonSerializerOptions.AllowTrailingCommas = true;
        //json.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
        //json.JsonSerializerOptions.Converters.Add(new NullableDateTimeJsonConverter());
    });

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("default");
builder.Services.AddDbContextPool<DbContext, PeopleDbContext>(db =>
{
    db.UseSqlServer(connectionString, options =>
    {
        options.EnableRetryOnFailure();
        options.MigrationsAssembly(typeof(PeopleDbContext).Assembly.GetName().Name);
    });

    db.UseSeeding((dbContext, _) =>
    {
        var peopleDbContext = (PeopleDbContext)dbContext;
        var defaultPerson = peopleDbContext.People.FirstOrDefault(p => p.Id == Person.Default.Id);
        if (defaultPerson == null)
        {
            peopleDbContext.Add(Person.Default);
            peopleDbContext.SaveChanges();
        }
        
    });

    db.UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
    {
        var peopleDbContext = (PeopleDbContext)dbContext;
        var defaultPerson = await peopleDbContext.People.FirstOrDefaultAsync(p => p.Id == Person.Default.Id, cancellationToken);
        if (defaultPerson == null)
        {
            peopleDbContext.Add(Person.Default);
            await peopleDbContext.SaveChangesAsync(cancellationToken);
        }
    });
});

builder.Services.AddHostedService<DatabaseMigrationBackgroundService>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();


/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory-1
/// </summary>
public partial class Program { }