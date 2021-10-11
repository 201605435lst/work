using Xunit;

namespace SnAbp.FeatureManagement.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class FeatureValueRepositoryTests : FeatureValueRepository_Tests<SnAbpFeatureManagementMongoDbTestModule>
    {

    }
}
