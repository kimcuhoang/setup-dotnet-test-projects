
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.Learn.AzureFunctionsTesting.Extension.DebugProcess.Core;
using Microsoft.VisualStudio.OLE.Interop;

namespace Example.AzureFunction.xUnit.DebuggerTools.DevTool;

public class VisualStudio : DevToolBase
{
    public async override Task AttachDebuggerAsync(int processId)
    {
        var process = this.GetProcess(processId);
        if (process == null)
        {
            return;
        }

        MessageFilter.Register();

        process.Attach();
        System.Diagnostics.Debug.WriteLine($"Automatically attached debugger to func.exe process {processId}");

        // isolated functions spin up yet another child process, so wait for the signal that it has started
        // and then try attaching to that process
        try
        {
            await this.WaitForSignalAsync(DebuggerConstants.SignalName, TimeSpan.FromSeconds(45));
            var isolatedProcess = this.FindIsolatedProcess();
            if (isolatedProcess != null)
            {
                isolatedProcess.Attach();
                System.Diagnostics.Debug.WriteLine($"Automatically attached debugger to isolated worker process");
            }
        }
        catch { }

        MessageFilter.Revoke();
    }

    private Task<bool> WaitForSignalAsync(string signalName, TimeSpan timeout)
    {
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

    private Process? GetProcess(int processID)
    {
        var processes = this.GetProcesses();
        return this.Try(() =>
        {
            existingProcessIds = processes?.Select(x => x.ProcessID).ToList();
            return processes?.SingleOrDefault(x => x.ProcessID == processID);
        });
    }

    private Process? FindIsolatedProcess()
    {
        var processes = this.GetProcesses();
        return this.Try(() =>
        {
            var newProcesses = processes?.ExceptBy(existingProcessIds!, x => x.ProcessID);
            return newProcesses?.SingleOrDefault(x => x.Name.EndsWith("dotnet.exe"));
        });
    }

    private IEnumerable<Process>? GetProcesses()
    {
        var thisVs = this.GetThisVsInstance();
        return this.Try(() =>
        {
            return thisVs?.Debugger.LocalProcesses.OfType<Process>();
        });
    }

    private DTE? GetThisVsInstance()
    {
        var vsInstances = this.GetInstances();
        return Try(() =>
        {
            var pid = System.Diagnostics.Process.GetCurrentProcess().Id;
            foreach (var vsInstance in vsInstances)
            {
                if (vsInstance.Debugger.DebuggedProcesses.Count > 0 && vsInstance.Debugger.CurrentMode == dbgDebugMode.dbgRunMode)
                {
                    foreach (var debuggedProcess in vsInstance.Debugger.DebuggedProcesses)
                    {
                        var debuggedProcessId = ((Process)debuggedProcess).ProcessID;
                        if (debuggedProcessId == pid)
                        {
                            return vsInstance;
                        }
                    }
                }
            }
            return null;
        });
    }

    private IEnumerable<DTE> GetInstances()
    {
        int retVal = GetRunningObjectTable(0, out IRunningObjectTable rot);

        if (retVal != 0)
        {
            yield break;
        }

        rot.EnumRunning(out IEnumMoniker enumMoniker);
        IMoniker[] moniker = new IMoniker[1];

        while (enumMoniker.Next(1, moniker, out uint fetched) == 0)
        {
            CreateBindCtx(0, out IBindCtx bindCtx);
            moniker[0].GetDisplayName(bindCtx, null, out string displayName);

            Console.WriteLine("Display Name: {0}", displayName);

            bool isVisualStudio = displayName.StartsWith("!VisualStudio");
            if (isVisualStudio)
            {
                rot.GetObject(moniker[0], out var obj);
                var dte = obj as DTE;
                yield return dte!;
            }
        }
    }

    [DllImport("ole32.dll")]
    private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

    [DllImport("ole32.dll")]
    private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
}
