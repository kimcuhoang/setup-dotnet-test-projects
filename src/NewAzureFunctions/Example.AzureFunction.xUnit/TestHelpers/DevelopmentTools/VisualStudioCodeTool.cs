
namespace Example.AzureFunction.xUnit.TestHelpers.DevelopmentTools;

public class VisualStudioCodeTool : DevelopmentToolBase
{
    protected override string ToolName => "Visual Studio Code";

    public override Task AttachToProcessAsync(int processId, Func<Task<bool>> waiter)
    {
        throw new NotImplementedException();
    }
}
