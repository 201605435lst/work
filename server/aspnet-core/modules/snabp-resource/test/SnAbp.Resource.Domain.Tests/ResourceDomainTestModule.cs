using SnAbp.Resource.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Resource
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(ResourceEntityFrameworkCoreTestModule)
        )]
    public class ResourceDomainTestModule : AbpModule
    {
        
    }
}
