using Microsoft.Extensions.Configuration;

namespace DNP.ThisIsAMainService.Tests.Setup.TestContainerBuilders;

public interface IHasConnectionString
{
    string ConnectionString { get; }
    void ConfigureConnectionString(IConfigurationBuilder configuration);
}