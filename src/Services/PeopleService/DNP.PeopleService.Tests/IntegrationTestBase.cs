
namespace DNP.PeopleService.Tests;
public class IntegrationTestBase(PersonalServiceTestCollectionFixture testCollectionFixture, ITestOutputHelper testOutput) : PeopleServiceTestBase(testCollectionFixture, testOutput)
{
}
