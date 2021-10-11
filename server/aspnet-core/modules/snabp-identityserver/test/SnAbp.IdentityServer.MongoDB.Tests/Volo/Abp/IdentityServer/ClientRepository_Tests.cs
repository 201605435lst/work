using Xunit;

namespace SnAbp.IdentityServer
{
    [Collection(MongoTestCollection.Name)]
    public class ClientRepository_Tests : ClientRepository_Tests<SnAbpIdentityServerMongoDbTestModule>
    {

    }
}
