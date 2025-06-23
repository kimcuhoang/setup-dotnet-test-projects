
namespace Example.AzureFunction.xUnit.TestHelpers.DevelopmentTools;

public abstract class DevelopmentToolBase
{
    protected List<int>? existingProcessIds = [];
    protected abstract string ToolName { get; }
    
    public abstract Task AttachToProcessAsync(int processId, Func<Task<bool>> waiter);

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
