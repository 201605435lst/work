using Xunit;

namespace SnAbp.TenantManagement.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class TenantRepository_Tests : TenantRepository_Tests<SnAbpTenantManagementMongoDbTestModule>
    {

    }
}
