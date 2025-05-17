using System.Diagnostics;
using Testcontainers.MsSql;
using Testcontainers.Xunit;
using Xunit.Sdk;

namespace DNP.PeopleService.Tests.xUnitV3;

public sealed class ServiceTestCollectionFixture(IMessageSink messageSink) : ContainerFixture<MsSqlBuilder, MsSqlContainer>(messageSink)
{
    public ServiceApplicationFactory Factory { get; private set; }
    protected override MsSqlBuilder Configure(MsSqlBuilder builder)
    {
        return builder
                .WithAutoRemove(true)
                .WithCleanUp(true)
                .WithHostname("test")
                .WithPortBinding(1433, assignRandomHostPort: true);
    }

    protected override async ValueTask InitializeAsync()
    {
        Debug.WriteLine($"{nameof(ServiceTestCollectionFixture)} {nameof(InitializeAsync)}");
        await base.InitializeAsync();
        this.Factory = new ServiceApplicationFactory(this.Container.GetConnectionString());
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore();
        await this.Factory.DisposeAsync();
        Debug.WriteLine($"{nameof(ServiceTestCollectionFixture)} {nameof(DisposeAsyncCore)}");
    }
}
