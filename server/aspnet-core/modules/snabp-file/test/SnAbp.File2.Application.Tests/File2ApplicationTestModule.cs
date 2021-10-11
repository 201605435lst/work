using Volo.Abp.Modularity;

namespace SnAbp.File
{
    [DependsOn(
        typeof(File2ApplicationModule),
        typeof(File2DomainTestModule)
    )]
    public class File2ApplicationTestModule : AbpModule
    {
    }
}