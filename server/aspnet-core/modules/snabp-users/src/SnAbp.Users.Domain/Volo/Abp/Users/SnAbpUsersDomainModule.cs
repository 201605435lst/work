using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace SnAbp.Users
{
    [DependsOn(
        typeof(SnAbpUsersDomainSharedModule),
        typeof(SnAbpUsersAbstractionModule),
        typeof(AbpDddDomainModule)
        )]
    public class SnAbpUsersDomainModule : AbpModule
    {
        
    }
}
