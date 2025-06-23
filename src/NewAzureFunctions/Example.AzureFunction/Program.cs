using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Learn.AzureFunctionsTesting.Extension.DebugProcess.Core;

if (OperatingSystem.IsWindows())
{
    DebugHelper.WaitForDebuggerToAttach();
}


var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Build().Run();

public partial class Program { }
