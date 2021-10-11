using SnAbp.File.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.File
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(File2EntityFrameworkCoreTestModule)
    )]
    public class File2DomainTestModule : AbpModule
    {
    }
}