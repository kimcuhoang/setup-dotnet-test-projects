namespace DNP.ThisIsAMainService.Tests.Setup;

[CollectionDefinition(nameof(DefaultTestCollection))]
public class DefaultTestCollection() : ICollectionFixture<TestCollectionFixture> { }