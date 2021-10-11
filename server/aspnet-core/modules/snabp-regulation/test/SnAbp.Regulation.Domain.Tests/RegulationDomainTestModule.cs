using SnAbp.Regulation.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Regulation
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(RegulationEntityFrameworkCoreTestModule)
        )]
    public class RegulationDomainTestModule : AbpModule
    {
        
    }
}
