namespace DNP.ThisIsAMainServices.Tests.TestImport;

[Collection(nameof(TestImportCollection))]
public abstract class TestImportBase(TestCollectionFixture collectionFixture, ITestOutputHelper testOutputHelper) : TestClassBase(collectionFixture, testOutputHelper)
{
}