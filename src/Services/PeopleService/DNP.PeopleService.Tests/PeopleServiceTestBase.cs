using Xunit.Abstractions;
using Xunit;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace DNP.PeopleService.Tests;

[Collection(nameof(PeopleServiceTestCollection))]
public abstract class PeopleServiceTestBase: IAsyncLifetime
{
    protected readonly PeopleServiceWebApplicationFactory _factory;
    protected readonly ITestOutputHelper _testOutput;
    protected readonly Faker _faker;

    protected PeopleServiceTestBase(PersonalServiceTestCollectionFixture testCollectionFixture, ITestOutputHelper testOutput)
    {
        this._factory = testCollectionFixture.Factory;
        this._testOutput = testOutput;
        this._faker = new Faker();

        Debug.WriteLine($"{nameof(PeopleServiceTestBase)} constructor");
    }

    protected Func<Faker, string> FakerPhoneNumber = faker => faker.Phone.PhoneNumber("## ### ####");

    protected async Task ExecuteServiceAsync(Func<IServiceProvider, Task> func)
    {
        await this._factory.ExecuteServiceAsync(func);
    }

    protected async Task ExecuteTransactionDbContextAsync(Func<DbContext, Task> func)
    {
        await this._factory.ExecuteServiceAsync(async serviceProvider =>
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
        await this._factory.ExecuteServiceAsync(async serviceProvider =>
        {
            var dbContext = serviceProvider.GetRequiredService<DbContext>();

            await func.Invoke(dbContext);
        });
    }

    protected async Task ExecuteHttpClientAsync(Func<HttpClient, Task> func)
    {
        using var httpClient = this._factory.CreateClient();
        await func.Invoke(httpClient);
    }

    public virtual async Task InitializeAsync()
    {
        await Task.Yield();
        Debug.WriteLine($"{nameof(PeopleServiceTestBase)} {nameof(InitializeAsync)}");
    }

    public virtual async Task DisposeAsync()
    {
        await Task.Yield();
        Debug.WriteLine($"{nameof(PeopleServiceTestBase)} {nameof(DisposeAsync)}");
    }
}