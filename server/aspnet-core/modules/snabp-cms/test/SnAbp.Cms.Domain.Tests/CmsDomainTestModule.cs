using SnAbp.Cms.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Cms
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(CmsEntityFrameworkCoreTestModule)
        )]
    public class CmsDomainTestModule : AbpModule
    {
        
    }
}
