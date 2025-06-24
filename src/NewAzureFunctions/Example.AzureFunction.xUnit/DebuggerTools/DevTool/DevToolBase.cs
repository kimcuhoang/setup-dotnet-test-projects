namespace Example.AzureFunction.xUnit.DebuggerTools.DevTool;

public abstract class DevToolBase
{
    protected List<int>? existingProcessIds = [];
    public abstract Task AttachDebuggerAsync(int processId);

    protected T Try<T>(Func<T> func)
    {
        while (true)
        {
            try
            {
                return func();
            }
            catch
            {

            }
        }
    }
}
