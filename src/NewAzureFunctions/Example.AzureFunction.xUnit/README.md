# An integration test for Azure Function

## Prerequisite
- .NET 9
- xUnit v2
- [Microsoft.Learn.AzureFunctionsTesting](https://github.com/microsoft/Microsoft.Learn.AzureFunctionsTesting)

## Limitation
- Just support only Window + Visual Studio

## Steps

### `Example.AzureFunction`
- Add [Microsoft.Learn.AzureFunctionsTesting.Extension.DebugProcess.Core](https://github.com/microsoft/Microsoft.Learn.AzureFunctionsTesting/blob/main/Microsoft.Learn.AzureFunctionsTesting.Core/README.md)
- From the `Program.cs`
    ```csharp
    if (OperatingSystem.IsWindows())
    {
        DebugHelper.WaitForDebuggerToAttach();
    }

    ```


### `Example.AzureFunction.xUnit`
- Add [Microsoft.Learn.AzureFunctionsTesting](https://github.com/microsoft/Microsoft.Learn.AzureFunctionsTesting/blob/main/Microsoft.Learn.AzureFunctionsTesting/README.md)
- Add [Microsoft.Learn.AzureFunctionsTesting.Extension.DebugProcess](https://github.com/microsoft/Microsoft.Learn.AzureFunctionsTesting/blob/main/Microsoft.Learn.AzureFunctionsTesting.Extension.DebugProcess/README.md)
