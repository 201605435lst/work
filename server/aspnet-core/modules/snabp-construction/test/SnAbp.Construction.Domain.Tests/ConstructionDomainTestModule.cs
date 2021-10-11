using SnAbp.Construction.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Construction
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(ConstructionEntityFrameworkCoreTestModule)
        )]
    public class ConstructionDomainTestModule : AbpModule
    {
        
    }
}
