using Xunit;

namespace SnAbp.IdentityServer
{
    [Collection(MongoTestCollection.Name)]
    public class IdentityResourceRepository_Tests : IdentityResourceRepository_Tests<SnAbpIdentityServerMongoDbTestModule>
    {
    }
}
