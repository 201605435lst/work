using SnAbp.StdBasic.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.StdBasic
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(StdBasicEntityFrameworkCoreTestModule)
        )]
    public class StdBasicDomainTestModule : AbpModule
    {
        
    }
}
