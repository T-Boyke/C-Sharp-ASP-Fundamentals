using Xunit;

namespace _10_Filmdatenbank.PlaywrightTests.Infrastructure
{
    [CollectionDefinition("SystemTestCollection")]
    public class SystemTestCollection : ICollectionFixture<TestHost<Program>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
