using System.Runtime.Versioning;
using Example.AzureFunction.xUnit.TestHelpers.DevelopmentTools;

namespace Example.AzureFunction.xUnit.TestHelpers.DebuggerTools;

[SupportedOSPlatform("windows")]
public class WindowsDebuggerTools: BaseDebuggerTools
{
    protected override bool CanExecute => OperatingSystem.IsWindows();

    protected override DevelopmentToolBase GetDevelopmentTool()
    {
        return new VisualStudioTool();
    }
}