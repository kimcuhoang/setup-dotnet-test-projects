# Playing with .NET

## Features

### Using [justfile](/.justfile) as a command runner
- [just](https://github.com/casey/just)
- [Justfile Cheat Sheet](https://cheatography.com/linux-china/cheat-sheets/justfile/)
- Installation
    - **Linux**: `curl -sSL instl.sh/casey/just/linux | bash`
    - **Windows**: `winget install --id Casey.Just --exact`

### Configure `dotnet-tool` in local
- Install dotnet's tools, i.e. `dotnet-ef` in local
    ```bash
    just dotnet-tools
    ```
    
### Configure **Kestrel**
- Via `appsettings.json`
- Then load via `Program.cs`
    ```csharp
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
        options.Configure(builder.Configuration.GetSection("Kestrel"));
    });
    ```
### EntityFrameworkCore
- Use [bundle migration](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#bundles)
- And [Data Seeding](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding)

### Setup an integration test project
- Using [xUnitV3](https://xunit.net/docs/getting-started/v3/whats-new)
- Using [Testcontainers.XunitV3](https://dotnet.testcontainers.org/test_frameworks/xunit_net/)
- Using [Testcontainers.MsSql](https://dotnet.testcontainers.org/modules/mssql/)
- Using [Microsoft Testing Platform support in xUnit.net v3](https://xunit.net/docs/getting-started/v3/microsoft-testing-platform)
- Highlighted:
    - `AssemblyFixture` as a [Shared Context between Tests](https://xunit.net/docs/shared-context) in which the database and application are created only once for all tests, which improves performance and reduces resource usage.
    - [Memory Configuration Provider](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#memory-configuration-provider) to dynamically change the settings for integration tests without modifying existing code
    - By using `WebApplicationFactory`, we can create a test server that runs the application in memory, which allows us to test the application without having to deploy it to a real server.

- Run tests via `just`
    ```bash
    just ms-test
    ```
    Or
    ```bash
    just test
    ```
### Github Action
- Use `just` to execute integration tests

## Give a Star! :star2:

If you liked this project or if it helped you, please give a star :star2: for this repository. Thank you!!!