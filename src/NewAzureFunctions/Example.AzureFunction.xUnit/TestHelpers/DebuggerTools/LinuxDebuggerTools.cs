using System.Runtime.Versioning;
using Example.AzureFunction.xUnit.TestHelpers.DevelopmentTools;

namespace Example.AzureFunction.xUnit.TestHelpers.DebuggerTools;

[SupportedOSPlatform("linux")]
public class LinuxDebuggerTools : BaseDebuggerTools
{
    protected override bool CanExecute => OperatingSystem.IsLinux();

    protected override DevelopmentToolBase GetDevelopmentTool()
    {
        return new VisualStudioCodeTool();
    }
}