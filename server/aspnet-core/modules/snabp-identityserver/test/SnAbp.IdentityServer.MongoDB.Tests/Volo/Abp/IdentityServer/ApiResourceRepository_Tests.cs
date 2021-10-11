using Xunit;

namespace SnAbp.IdentityServer
{
    [Collection(MongoTestCollection.Name)]
    public class ApiResourceRepository_Tests : ApiResourceRepository_Tests<SnAbpIdentityServerMongoDbTestModule>
    {
    }
}
