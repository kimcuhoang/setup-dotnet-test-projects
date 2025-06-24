
using System.Diagnostics;
using Example.AzureFunction.xUnit.DebuggerTools.OSRunner;
using Shouldly;

namespace Example.AzureFunction.xUnit.DebuggerTools;

public class WorkingSession
{
    public OSBase WorkingOS { get; }
    public HttpClient Client { get; }

    private Process? hostProcess;
    private const int PORT = 7071;
    private const bool ENABLED_AUTH = false;

    public WorkingSession()
    {
        this.Client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:7071/"),
            Timeout = TimeSpan.FromMinutes(5)
        };

        this.WorkingOS = OperatingSystem.IsWindows()
            ? new WindowsOS()
            : OperatingSystem.IsLinux()
                ? new LinuxOS()
                : throw new NotSupportedException("Operating system not supported");
    }

    public async Task InitializeAsync()
    {
        var functionAppPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
        Directory
            .Exists(functionAppPath)
            .ShouldBeTrue($"The function app path '{functionAppPath}' does not exist. Please ensure the path is correct.");

        // Kill any existing func processes// Kill any existing func processes
        foreach (var process in Process.GetProcessesByName("func"))
        {
            try
            {
                process.Kill(true);
                process.WaitForExit();
            }
            catch { /* Ignore exceptions for processes that may have exited */ }
        }


        hostProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "func",
                Arguments = $"start -p {PORT}" + (ENABLED_AUTH ? " --enableAuth" : ""),
                WorkingDirectory = functionAppPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        var envVars = new Dictionary<string, string>
        {
            { "IS_FUNCTIONS_TEST", Boolean.TrueString }
        };

        if (Debugger.IsAttached)
        {
            envVars["__WAIT_FOR_DEBUGGER__"] = Boolean.TrueString;
        }

        if (!hostProcess.Start())
        {
            throw new InvalidOperationException("Could not start Azure Functions host.");
        }

        if (Debugger.IsAttached)
        {
            await this.AttachDebuggerAsync(this.hostProcess.Id);
        }

        await this.EnsureTheHostToBeReady();
    }

    public async Task DisposeAsync()
    {
        if (hostProcess == null)
        {
            return;
        }

        if (!hostProcess.HasExited)
        {
            hostProcess.Kill();
        }

        hostProcess.Dispose();
        
        await Task.Yield();
    }

    public async Task AttachDebuggerAsync(int processId)
    {
        if (OperatingSystem.IsWindows()) return;

        var devTool = this.WorkingOS.GetDevTool()
                ?? throw new InvalidOperationException("DevTool is not available for the current OS.");

        await devTool.AttachDebuggerAsync(processId);
    }
    
    private async Task EnsureTheHostToBeReady()
    {
        // Wait for the host to be ready
        var ready = false;
        var timeout = TimeSpan.FromSeconds(15);
        var startTime = DateTime.UtcNow;

        while (!ready && DateTime.UtcNow - startTime < timeout)
        {
            try
            {
                var response = await this.Client.GetAsync("");
                ready = ((int)response.StatusCode) < 500;
            }
            catch
            {
                // Ignore and retry
            }
            if (!ready)
            {
                await Task.Delay(1000);
            }
        }

        if (!ready)
        {
            throw new Exception($"The Functions Host Runtime did not start properly after {timeout.TotalSeconds} seconds.");
        }
    }
}
