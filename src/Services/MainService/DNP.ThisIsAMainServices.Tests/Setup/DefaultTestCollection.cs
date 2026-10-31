namespace DNP.ThisIsAMainServices.Tests.Setup;

[CollectionDefinition(nameof(DefaultTestCollection))]
public class DefaultTestCollection() : ICollectionFixture<TestCollectionFixture> { }