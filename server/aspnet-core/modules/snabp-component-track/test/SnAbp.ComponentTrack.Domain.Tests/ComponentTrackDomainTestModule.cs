using SnAbp.ComponentTrack.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.ComponentTrack
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(ComponentTrackEntityFrameworkCoreTestModule)
        )]
    public class ComponentTrackDomainTestModule : AbpModule
    {
        
    }
}
