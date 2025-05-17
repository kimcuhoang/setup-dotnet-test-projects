# setup-dotnet-test-projects

An introduction how to setup and organize the test projects in .NET


## Features

- [MSSQL TestContainer](https://testcontainers.com/modules/mssql/) in order to isolating database from integration test with others.
- [Memory Configuration Provider](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#memory-configuration-provider) to override the settings for testing

## First

1. Define `PeopleServiceWebApplicationFactory` which is a custom of `WebApplicationFactory`
2. Define `PeopleServiceTestFixture` which is an implementation of `IAsyncLifetime`
3. Define `PeopleServiceTestBase` the base class of every test

## Then implement tests

1. Define `CreatePersonTestFixture.cs` inherits from `PeopleServiceTestFixture`
2. Define `DoTestCreatePerson` inherits from `PeopleServiceTestBase`


## Resources

- [Shared Context between Tests](https://xunit.net/docs/shared-context)
- [Integration Testing with xUnit](https://www.jimmybogard.com/integration-testing-with-xunit/)
- [How to create Parameterized Tests with xUnit](https://davecallan.com/creating-parameterized-tests-xunit/)
- [Using Testing.Platform with NET 9](https://dateo-software.de/blog/testing-platform)
