namespace SnAbp.Users.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class AbpUserRepository_Tests : AbpUserRepository_Tests<AbpUsersMongoDbTestModule>
    {

    }
}
