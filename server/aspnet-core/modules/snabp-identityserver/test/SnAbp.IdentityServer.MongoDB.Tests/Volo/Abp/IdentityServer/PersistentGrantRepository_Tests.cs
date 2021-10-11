using Xunit;

namespace SnAbp.IdentityServer
{
    [Collection(MongoTestCollection.Name)]
    public class PersistentGrantRepository_Tests : PersistentGrantRepository_Tests<SnAbpIdentityServerMongoDbTestModule>
    {

    }
}
