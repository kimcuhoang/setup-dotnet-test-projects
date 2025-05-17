# How to setup an integration test project with xUnit and TestContainers in .NET

This project demonstrates how to set up an integration test project using xUnit and TestContainers in .NET. The highlights of this project are:
- xUnit V3 and Microsoft Testing Platform support
- TestContainers with xUnit V3
- MSSQL TestContainer
- Only one instance of 

## Features

- By using `WebApplicationFactory`, we can create a test server that runs the application in memory, which allows us to test the application without having to deploy it to a real server.
- By using `TestContainers`, we can create a test database that runs in a Docker container, which allows us to test the application with a real database without having to install and configure a database server on the local machine.
- By using `AssemblyFixture`, we can ensure that the database and application are created only once for all tests, which improves performance and reduces resource usage.
- By using `IAsyncLifetime`, we can ensure that the test database is created and destroyed automatically, which simplifies the test setup and teardown process.
- By using `IConfiguration`, we can easily configure the test database connection string and other settings, which allows us to customize the test environment without having to modify the application code.


## First

1. Define `ServiceApplicationFactory` which is a custom of `WebApplicationFactory`
2. Define `ServiceTestAssemblyFixture` which is an derived class of `ContainerFixture` from `TestContainers.xUnitV3`
3. Define `ServiceTestBase` the base class of every test and implement `IAsyncLifetime`

## Then implement tests

1. Define your test class (i.e. `DoTestCreatePerson`) which must inherit from `PeopleServiceTestBase`


## Resources

- [Shared Context between Tests](https://xunit.net/docs/shared-context)
- [Integration Testing with xUnit](https://www.jimmybogard.com/integration-testing-with-xunit/)
- [How to create Parameterized Tests with xUnit](https://davecallan.com/creating-parameterized-tests-xunit/)
- [Using Testing.Platform with NET 9](https://dateo-software.de/blog/testing-platform)
- [Microsoft Testing Platform support in xUnit.net v3](https://xunit.net/docs/getting-started/v3/microsoft-testing-platform)
- [How to use Testcontainers with .NET Unit Tests](https://blog.jetbrains.com/dotnet/2023/10/24/how-to-use-testcontainers-with-dotnet-unit-tests/)
- [Testing with xUnit.net](https://dotnet.testcontainers.org/test_frameworks/xunit_net/)
- [Memory Configuration Provider](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#memory-configuration-provider)
