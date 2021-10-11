using SnAbp.Technology.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Technology
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(TechnologyEntityFrameworkCoreTestModule)
        )]
    public class TechnologyDomainTestModule : AbpModule
    {
        
    }
}
