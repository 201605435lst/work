using SnAbp.Oa.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Oa
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(OaEntityFrameworkCoreTestModule)
        )]
    public class OaDomainTestModule : AbpModule
    {
        
    }
}
