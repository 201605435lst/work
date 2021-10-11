using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Users.EntityFrameworkCore
{
    [DependsOn(
        typeof(SnAbpUsersDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class SnAbpUsersEntityFrameworkCoreModule : AbpModule
    {
        
    }
}
