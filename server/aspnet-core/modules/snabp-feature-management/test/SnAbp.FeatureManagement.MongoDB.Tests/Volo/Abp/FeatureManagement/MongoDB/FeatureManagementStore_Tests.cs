using Xunit;

namespace SnAbp.FeatureManagement.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class FeatureManagementStore_Tests : FeatureManagementStore_Tests<SnAbpFeatureManagementMongoDbTestModule>
    {

    }
}
