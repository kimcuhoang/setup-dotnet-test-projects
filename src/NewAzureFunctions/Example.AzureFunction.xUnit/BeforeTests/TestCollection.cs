namespace Example.AzureFunction.xUnit.BeforeTests;

[CollectionDefinition(nameof(TestCollection))]
public class TestCollection : ICollectionFixture<TestCollectionFixture>
{
}