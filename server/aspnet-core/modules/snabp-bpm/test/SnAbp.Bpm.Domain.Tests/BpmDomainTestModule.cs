using SnAbp.Bpm.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Bpm
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(BpmEntityFrameworkCoreTestModule)
        )]
    public class BpmDomainTestModule : AbpModule
    {
        
    }
}
