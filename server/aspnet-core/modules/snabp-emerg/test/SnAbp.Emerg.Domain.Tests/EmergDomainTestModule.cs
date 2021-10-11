using SnAbp.Emerg.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Emerg
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(EmergEntityFrameworkCoreTestModule)
        )]
    public class EmergDomainTestModule : AbpModule
    {
        
    }
}
