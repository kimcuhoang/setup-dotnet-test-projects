using DNP.PeopleService.Tests.xUnitV3;
using System.Diagnostics;
using Testcontainers.MsSql;
using Testcontainers.Xunit;
using Xunit.Sdk;

[assembly: AssemblyFixture(typeof(ServiceTestAssemblyFixture))]

namespace DNP.PeopleService.Tests.xUnitV3;

public sealed class ServiceTestAssemblyFixture(IMessageSink messageSink) : ContainerFixture<MsSqlBuilder, MsSqlContainer>(messageSink)
{
    private const string SqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest";
    public ServiceApplicationFactory Factory { get; private set; }

    protected override MsSqlBuilder Configure()
    {
        return new MsSqlBuilder(SqlServerImage)
                .WithAutoRemove(true)
                .WithCleanUp(true)
                .WithHostname("test")
                .WithPortBinding(1433, assignRandomHostPort: true);
    }

    protected override async ValueTask InitializeAsync()
    {
        Debug.WriteLine($"{nameof(ServiceTestAssemblyFixture)} {nameof(InitializeAsync)}");
        await base.InitializeAsync();
        this.Factory = new ServiceApplicationFactory(this.Container.GetConnectionString());
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore();
        await this.Factory.DisposeAsync();
        Debug.WriteLine($"{nameof(ServiceTestAssemblyFixture)} {nameof(DisposeAsyncCore)}");
    }
}
