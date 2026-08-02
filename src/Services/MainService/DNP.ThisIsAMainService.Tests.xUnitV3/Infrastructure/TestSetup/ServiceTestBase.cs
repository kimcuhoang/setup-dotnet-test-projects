using DNP.PeopleService.Tests.xUnitV3.Infrastructure.HttpDelegatingHandlers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DNP.PeopleService.Tests.xUnitV3.Infrastructure.TestSetup;

public abstract class ServiceTestBase(ServiceTestAssemblyFixture testAssemblyFixture, ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
    protected readonly ServiceApplicationFactory Factory = testAssemblyFixture.Factory;

    protected readonly ITestOutputHelper TestOutputHelper = testOutputHelper;

    protected readonly IServiceProvider ServiceProvider = testAssemblyFixture.Factory.Services;

    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected readonly Faker Faker = new();

    public virtual async ValueTask DisposeAsync()
    {
        await this.ExecuteDbContextAsync(async dbContext =>
        {
            await dbContext.Set<PersonDomain>()
                .Where(_ => _.Id != PersonDomain.Default.Id)
                .ExecuteDeleteAsync(this.CancellationToken);
        });
    }

    public virtual ValueTask InitializeAsync() => ValueTask.CompletedTask;

    protected async Task ExecuteTransactionDbContextAsync(Func<DbContext, Task> func)
    {
        await this.Factory.ExecuteServiceAsync(async serviceProvider =>
        {
            var dbContext = serviceProvider.GetRequiredService<DbContext>();

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                // Achieving atomicity
                await using var transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await func.Invoke(dbContext);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });
    }

    protected async Task ExecuteDbContextAsync(Func<DbContext, Task> func)
    {
        await this.Factory.ExecuteServiceAsync(async serviceProvider =>
        {
            var dbContext = serviceProvider.GetRequiredService<DbContext>();

            await func.Invoke(dbContext);
        });
    }

    protected async Task ExecuteHttpClientAsync(Func<HttpClient, Task> func)
    {
        var server = testAssemblyFixture.Factory.Server;

        var handler = server.CreateHandler();

        var loggerDelegatingHandler = new HttpLoggerDelegatingHandler(this.TestOutputHelper)
        {
            InnerHandler = handler
        };

        using var httpClient = new HttpClient(loggerDelegatingHandler)
        {
            BaseAddress = server.BaseAddress
        };

        await func.Invoke(httpClient);
    }

    public HttpClient GetCustomHttpClient()
    {
        var server = testAssemblyFixture.Factory.Server;

        var handler = server.CreateHandler();

        var loggerDelegatingHandler = new HttpLoggerDelegatingHandler(this.TestOutputHelper)
        {
            InnerHandler = handler
        };

        var httpClient = new HttpClient(loggerDelegatingHandler)
        {
            BaseAddress = server.BaseAddress
        };

        return httpClient;
    }

}
