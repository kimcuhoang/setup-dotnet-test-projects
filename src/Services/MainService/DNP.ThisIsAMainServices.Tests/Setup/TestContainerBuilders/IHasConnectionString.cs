using Microsoft.Extensions.Configuration;

namespace DNP.ThisIsAMainServices.Tests.Setup.TestContainerBuilders;

public interface IHasConnectionString
{
    string ConnectionString { get; }
    void ConfigureConnectionString(IConfigurationBuilder configuration);
}