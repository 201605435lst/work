using SnAbp.Material.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Material
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(MaterialEntityFrameworkCoreTestModule)
        )]
    public class MaterialDomainTestModule : AbpModule
    {
        
    }
}
