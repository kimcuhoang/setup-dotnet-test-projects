namespace DNP.ThisIsAMainServices.TestsV1.Setup;

[CollectionDefinition(nameof(DefaultTestCollection))]
public class DefaultTestCollection() : ICollectionFixture<TestCollectionFixture> { }