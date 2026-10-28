using Microsoft.Extensions.Configuration;

namespace DNP.ThisIsAMainServices.TestsV1.Setup.TestContainerBuilders;

public interface IHasConnectionString
{
    string ConnectionString { get; }
    void ConfigureConnectionString(IConfigurationBuilder configuration);
}