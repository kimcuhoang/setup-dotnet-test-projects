using Example.AzureFunction.xUnit.TestHelpers.DevelopmentTools;
using Microsoft.Learn.AzureFunctionsTesting.Extension.DebugProcess.Core;

namespace Example.AzureFunction.xUnit.TestHelpers.DebuggerTools;

public abstract class BaseDebuggerTools
{
    protected abstract bool CanExecute { get; }
    protected abstract DevelopmentToolBase GetDevelopmentTool();

    public async Task AttachToProcessAsync(int processId)
    {
        var waiter = WaitForSignalAsync(DebuggerConstants.SignalName, TimeSpan.FromSeconds(45));
        MessageFilter.Register();

        var developmentTool = GetDevelopmentTool();
        await developmentTool.AttachToProcessAsync(processId, async () => await waiter);

        MessageFilter.Revoke();
    }

    protected Task<bool> WaitForSignalAsync(string signalName, TimeSpan timeout)
    {
        if (!this.CanExecute)
        {
            throw new PlatformNotSupportedException();
        }

        using var ewh = new EventWaitHandle(false, EventResetMode.ManualReset, signalName);

        // optimize for special cases
        var alreadySignalled = ewh.WaitOne(0);
        if (alreadySignalled)
            return Task.FromResult(true);
        if (timeout == TimeSpan.Zero)
            return Task.FromResult(false);

        var tcs = new TaskCompletionSource<bool>();
        var threadPoolRegistration = ThreadPool.RegisterWaitForSingleObject(ewh, (state, timedOut) => ((TaskCompletionSource<bool>?)state)?.TrySetResult(!timedOut), tcs, timeout, true);
        return tcs.Task;
    }
}
