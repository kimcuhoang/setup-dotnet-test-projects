using Example.AzureFunction.xUnit.DebuggerTools.DevTool;

namespace Example.AzureFunction.xUnit.DebuggerTools.OSRunner;

public abstract class OSBase
{
    public abstract DevToolBase GetDevTool();
}
