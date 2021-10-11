using SnAbp.Common.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Common
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(CommonEntityFrameworkCoreTestModule)
        )]
    public class CommonDomainTestModule : AbpModule
    {
        
    }
}
