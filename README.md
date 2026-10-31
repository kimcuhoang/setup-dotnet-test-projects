# ASP.NET Core Web API integration tests

A practical example of testing an ASP.NET Core Web API with xUnit v3 and Testcontainers.

This project uses [Microsoft Testing Platform](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro), a lightweight platform for running and managing .NET tests.

## Test setup

```text
Test classes without explicit collections
                 |
                 v
     AutoBucketCollectionFactory
          /              \
         v                v
 Dynamic collection  Dynamic collection
         \                /
          v              v
        TestCollectionFixture
        (one per collection)
                 |
                 v
       ASP.NET Core test server
                 ^
                 | connection string
       TestAssemblyFixture
                 |
                 v
      SQL Server Testcontainer
       (started once per assembly)
```

- `TestAssemblyFixture` starts a SQL Server container once for the test assembly.
- `TestCollectionFixture` creates the test application and configures it with the container's connection string.
- `AutoBucketCollectionFactory` distributes test classes without an explicit collection across dynamic collections, helping tests run in parallel.
- Tests run against the application and its SQL Server database.

## Run tests

```bash
dotnet test NET.slnx
```

Docker must be running so Testcontainers can start SQL Server.
