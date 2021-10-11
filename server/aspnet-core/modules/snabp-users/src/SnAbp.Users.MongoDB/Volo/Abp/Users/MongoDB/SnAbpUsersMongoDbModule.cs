using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace SnAbp.Users.MongoDB
{
    [DependsOn(
        typeof(SnAbpUsersDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class SnAbpUsersMongoDbModule : AbpModule
    {
        
    }
}
