namespace DNP.ThisIsAMainServices.Tests.Setup.TestContainerBuilders;

public interface ITestContainerBuilder
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}