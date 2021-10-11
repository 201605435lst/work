using SnAbp.Quality.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Quality
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(QualityEntityFrameworkCoreTestModule)
        )]
    public class QualityDomainTestModule : AbpModule
    {
        
    }
}
