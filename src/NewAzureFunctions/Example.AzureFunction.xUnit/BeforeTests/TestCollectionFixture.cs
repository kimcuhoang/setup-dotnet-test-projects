using System.Diagnostics;
using Example.AzureFunction.xUnit.WindowsTestHelpers;
using Shouldly;

namespace Example.AzureFunction.xUnit.BeforeTests;

public class TestCollectionFixture: IAsyncLifetime
{
    private Process? hostProcess;
    public HttpClient Client { get; }

    public TestCollectionFixture()
    {
        this.Client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:7071/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
    }

    public async Task InitializeAsync()
    {
        var functionAppPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
        Directory
            .Exists(functionAppPath)
            .ShouldBeTrue($"The function app path '{functionAppPath}' does not exist. Please ensure the path is correct.");

        // Kill any existing func processes
        foreach (var process in Process.GetProcessesByName("func"))
        {
            try
            {
                process.Kill(true);
                process.WaitForExit();
            }
            catch { /* Ignore exceptions for processes that may have exited */ }
        }

        const int port = 7071;
        const bool enabledAuth = false;
        hostProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "func",
                Arguments = $"start -p {port}" + (enabledAuth ? " --enableAuth" : ""),
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

        if (OperatingSystem.IsWindows())
        {
            if (Debugger.IsAttached)
            {
                envVars["__WAIT_FOR_DEBUGGER__"] = Boolean.TrueString;
            }
        }

        if (!hostProcess.Start())
        {
            throw new InvalidOperationException("Could not start Azure Functions host.");
        }

        if (OperatingSystem.IsWindows())
        {
            if (Debugger.IsAttached)
            {
                await WindowsDebuggerTools.AttachToProcessAsync(this.hostProcess.Id);
            }
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
