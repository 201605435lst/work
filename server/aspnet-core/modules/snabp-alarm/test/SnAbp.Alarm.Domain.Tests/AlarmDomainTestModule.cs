using SnAbp.Alarm.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Alarm
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(AlarmEntityFrameworkCoreTestModule)
        )]
    public class AlarmDomainTestModule : AbpModule
    {
        
    }
}
