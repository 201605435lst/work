using Volo.Abp.Modularity;

namespace SnAbp.Quality
{
    [DependsOn(
        typeof(QualityApplicationModule),
        typeof(QualityDomainTestModule)
        )]
    public class QualityApplicationTestModule : AbpModule
    {

    }
}
