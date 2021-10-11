using Volo.Abp.Modularity;

namespace SnAbp.Quality
{
    [DependsOn(
        typeof(QualityDomainSharedModule)
        )]
    public class QualityDomainModule : AbpModule
    {

    }
}
