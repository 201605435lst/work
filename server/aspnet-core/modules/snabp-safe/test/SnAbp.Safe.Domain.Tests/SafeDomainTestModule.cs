using SnAbp.Safe.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Safe
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(SafeEntityFrameworkCoreTestModule)
        )]
    public class SafeDomainTestModule : AbpModule
    {
        
    }
}
